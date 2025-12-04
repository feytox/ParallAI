using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Util;

namespace ParallAI.Infrastructure.ValueTypes;

[JsonConverter(typeof(JsonWebEnumConverter<OpenAiReasoningEffort>))]
public enum OpenAiReasoningEffort
{
    None,
    Minimal,
    Low,
    Medium,
    High
}

public static class OpenAiReasoningEffortExt
{
    public static OpenAiReasoningEffort ToOpenAi(this ThinkingBudget? budget)
    {
        return budget switch
        {
            null or ThinkingBudget.Unknown  or ThinkingBudget.None => OpenAiReasoningEffort.None,
            ThinkingBudget.Dynamic => OpenAiReasoningEffort.Medium,
            ThinkingBudget.Minimal => OpenAiReasoningEffort.Minimal,
            ThinkingBudget.Low => OpenAiReasoningEffort.Low,
            ThinkingBudget.Medium => OpenAiReasoningEffort.Medium,
            ThinkingBudget.High => OpenAiReasoningEffort.High,
            _ => throw new ArgumentOutOfRangeException(nameof(budget), budget, null)
        };
    }
}