using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

public record OpenRouterRequest(
    string Model,
    OpenAiMessage[] Messages,
    decimal Temperature,
    OpenRouterRequest.ReasoningConfig? Reasoning = null)
{
    public static OpenRouterRequest Create(string modelId, IEnumerable<OpenAiMessage> messages,
        PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            messages = new [] {OpenAiMessage.CreateSystemInstruction(promptSettings)}.Concat(messages);
        var effort = promptSettings.ThinkingBudget.ToOpenAi();
        var reasoning = new EffortReasoning(effort);
        return new OpenRouterRequest(modelId, messages.ToArray(), promptSettings.Temperature, reasoning);
    }
    
    [JsonDerivedType(typeof(MaxTokensReasoning))]
    [JsonDerivedType(typeof(EffortReasoning))]
    public abstract record ReasoningConfig;

    public record MaxTokensReasoning([property: JsonPropertyName("max_tokens")] int MaxTokens) : ReasoningConfig;

    public record EffortReasoning(OpenAiReasoningEffort Effort) : ReasoningConfig;
}