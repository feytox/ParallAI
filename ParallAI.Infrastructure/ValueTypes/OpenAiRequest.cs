using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI API Reference</see>
/// </remarks>
public record OpenAiRequest(
    string Model,
    OpenAiMessage[] Messages,
    decimal Temperature,
    [property: JsonPropertyName("reasoning_effort")]
    OpenAiReasoningEffort ReasoningEffort
)
{
    public static OpenAiRequest Create(string modelId, IEnumerable<OpenAiMessage> messages,
        PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            messages = messages.Prepend(OpenAiMessage.CreateSystemInstruction(promptSettings));
        var effort = promptSettings.ThinkingBudget.ToOpenAi();
        return new OpenAiRequest(modelId, messages.ToArray(), promptSettings.Temperature, effort);
    }
}