using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

public record OpenAiMessage(
    string Content,
    [property: JsonConverter(typeof(JsonWebEnumConverter<OpenAiMessage.MessageRole>))]
    OpenAiMessage.MessageRole Role)
{
    public static IEnumerable<OpenAiMessage> CreateMessages(Prompt prompt, PromptSettings promptSettings)
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