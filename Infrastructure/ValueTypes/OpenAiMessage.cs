using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create#chat_create-messages">OpenAI API Reference</see>
/// </remarks>
public record OpenAiMessage(
    string Content,
    [property: JsonConverter(typeof(JsonWebEnumConverter<OpenAiMessage.MessageRole>))]
    OpenAiMessage.MessageRole Role)
{
    public static IEnumerable<OpenAiMessage> CreateMessages(TextPrompt prompt, PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            yield return new OpenAiMessage(promptSettings.SystemInstructions, MessageRole.System);

        yield return new OpenAiMessage(prompt.Text, MessageRole.User);
    }

    public enum MessageRole
    {
        User,
        Assistant,
        System
    }
}