using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create#chat_create-messages">OpenAI API Reference</see>
/// </remarks>
public record OpenAiMessage(OpenAiContent Content, OpenAiMessage.MessageRole Role)
{
    public static IEnumerable<OpenAiMessage> Create(TextPrompt prompt, PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            yield return CreateSystemInstruction(promptSettings);

        yield return new OpenAiMessage(OpenAiContent.Create(prompt.Text), MessageRole.User);
    }

    public static IEnumerable<OpenAiMessage> Create(FilePrompt prompt, IEnumerable<AiFile> files,
        PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            yield return CreateSystemInstruction(promptSettings);

        yield return new OpenAiMessage(OpenAiContent.Create(prompt.Text, files), MessageRole.User);
    }

    private static OpenAiMessage CreateSystemInstruction(PromptSettings promptSettings)
    {
        return new OpenAiMessage(OpenAiContent.Create(promptSettings.SystemInstructions), MessageRole.System);
    }

    [JsonConverter(typeof(JsonWebEnumConverter<MessageRole>))]
    public enum MessageRole
    {
        User,
        Assistant,
        System
    }
}