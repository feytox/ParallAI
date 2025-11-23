using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Util;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create#chat_create-messages">OpenAI API Reference</see>
/// </remarks>
public record OpenAiMessage(OpenAiContent Content, OpenAiMessage.MessageRole Role)
{
    public static OpenAiMessage CreateSystemInstruction(PromptSettings promptSettings)
    {
        return new OpenAiMessage(OpenAiContent.Create(promptSettings.SystemPrompt), MessageRole.System);
    }

    [JsonConverter(typeof(JsonWebEnumConverter<MessageRole>))]
    public enum MessageRole
    {
        User,
        Assistant,
        System
    }
}

public static class OpenAiMappings
{
    private static OpenAiMessage.MessageRole ToOpenAiRole(this Role role)
    {
        return role switch
        {
            Role.User => OpenAiMessage.MessageRole.User,
            Role.Assistant => OpenAiMessage.MessageRole.Assistant,
            _ => throw new ArgumentException($"Unknown role: {role}")
        };
    }
    
    public static OpenAiMessage ToOpenAiMessage(this TextMessage message) 
        => new OpenAiMessage(OpenAiContent.Create(message.Text), message.Role.ToOpenAiRole());
    
    public static OpenAiMessage ToOpenAiMessage(this FileMessage message, IEnumerable<AiFile> files) =>
        new OpenAiMessage(OpenAiContent.Create(message.Text, files), message.Role.ToOpenAiRole());
}