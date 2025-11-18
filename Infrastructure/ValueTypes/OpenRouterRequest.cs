using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

public record OpenRouterRequest(
    string Model,
    OpenAiMessage[] Messages,
    decimal Temperature,
    OpenRouterRequest.ReasoningConfig? Reasoning = null)
{
    public static OpenRouterRequest Create(string modelId, TextPrompt prompt, PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(prompt, promptSettings).ToArray();
        return new OpenRouterRequest(modelId, messages, promptSettings.Temperature);
    }

    public static OpenRouterRequest Create(string modelId, FilePrompt prompt, IEnumerable<AiFile> files,
        PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(prompt, files, promptSettings).ToArray();
        return new OpenRouterRequest(modelId, messages, promptSettings.Temperature);
    }
    
    public abstract record ReasoningConfig;

    public record MaxTokensReasoning(
        [property: JsonPropertyName("max_tokens")]
        int MaxTokens);

    public record EffortReasoning(
        [property: JsonConverter(typeof(JsonWebEnumConverter<GeminiContent.MessageRole>))]
        Effort Effort);

    public enum Effort
    {
        Low,
        Medium,
        High
    }
}