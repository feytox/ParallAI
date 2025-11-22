using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

public record OpenRouterRequest(
    string Model,
    OpenAiMessage[] Messages,
    decimal Temperature,
    OpenRouterRequest.ReasoningConfig? Reasoning = null)
{
    public static OpenRouterRequest Create(string modelId, TextMessage message, PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(message, promptSettings).ToArray();
        var effort = promptSettings.ThinkingBudget.ToOpenAi();
        var reasoning = new EffortReasoning(effort);
        return new OpenRouterRequest(modelId, messages, promptSettings.Temperature, reasoning);
    }

    public static OpenRouterRequest Create(string modelId, FileMessage message, IEnumerable<AiFile> files,
        PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(message, files, promptSettings).ToArray();
        var effort = promptSettings.ThinkingBudget.ToOpenAi();
        var reasoning = new EffortReasoning(effort);
        return new OpenRouterRequest(modelId, messages, promptSettings.Temperature, reasoning);
    }
    
    [JsonDerivedType(typeof(MaxTokensReasoning))]
    [JsonDerivedType(typeof(EffortReasoning))]
    public abstract record ReasoningConfig;

    public record MaxTokensReasoning([property: JsonPropertyName("max_tokens")] int MaxTokens) : ReasoningConfig;

    public record EffortReasoning(OpenAiReasoningEffort Effort) : ReasoningConfig;
}