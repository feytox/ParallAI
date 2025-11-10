using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

public record OpenRouterRequest(
    string Model,
    OpenAiMessage[] Messages,
    double Temperature,
    OpenRouterRequest.ReasoningConfig? Reasoning = null)
{
    public static OpenRouterRequest Create(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.CreateMessages(prompt, promptSettings).ToArray();
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