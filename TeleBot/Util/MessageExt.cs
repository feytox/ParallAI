using Infrastructure.ValueTypes;
using Telegram.Bot.Types;

namespace TeleBot.Util;

public static class MessageExt
{
    public static AiFileInfo? GetFileInfo(this Message message)
    {
        if (message.Document is not null)
            return GetDocumentInfo(message.Document);

        if (message.Photo is not null)
            return GetPhotoInfo(message.Photo);
        
        return null;
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