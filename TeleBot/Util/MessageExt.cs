using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBot.Util;

public static class MessageExt
{
    public static bool IsMediaGroup(this Message message)
    {
        return message.MediaGroupId is not null;
    }
    
    public static Prompt CreatePrompt(Message[] messages)
    {
        if (messages.Length == 1)
            return CreatePrompt(messages[0]);

        var files = messages
            .Select(GetFileInfo)
            .ToArray();

        var caption = messages
            .Select(message => message.Caption)
            .FirstOrDefault();
        return FilePrompt.Create(files, caption);
    }

    private static Prompt CreatePrompt(Message message)
    {
        return message.Type switch
        {
            MessageType.Text => new TextPrompt(message.Text!),
            MessageType.Photo or MessageType.Document => FilePrompt.Create([message.GetFileInfo()], message.Caption),
            _ => throw new ArgumentOutOfRangeException($"Unsupported message type: {message.Type}")
        };
    }

    private static AiFileInfo GetFileInfo(this Message message)
    {
        if (message.Document is not null)
            return GetDocumentInfo(message.Document);

        if (message.Photo is not null)
            return GetPhotoInfo(message.Photo);

        throw new NullReferenceException("The message does not contain supported files.");
    }

    private static AiFileInfo GetDocumentInfo(Document document)
    {
        if (document.MimeType is null)
            throw new NullReferenceException("The document's MimeType must not be null.");

        return new AiFileInfo(document.FileId, document.MimeType);
    }

    private static AiFileInfo GetPhotoInfo(PhotoSize[] photoSizes)
    {
        var photo = photoSizes[^1];
        return new AiFileInfo(photo.FileId, "image/jpeg");
    }
}