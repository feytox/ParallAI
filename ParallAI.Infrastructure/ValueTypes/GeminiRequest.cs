using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using static ParallAI.Core.ValueTypes.ThinkingBudget;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public record GeminiRequest(
    GeminiContent[] Contents,
    [property: JsonPropertyName("system_instruction")]
    GeminiContent SystemInstruction,
    GeminiRequest.GenConfig GenerationConfig)
{
    public static GeminiRequest Create(IEnumerable<GeminiContent> messages, PromptSettings settings)
    {
        var thinkingConfig = ThinkingConfig.Create(settings.ThinkingBudget);
        return new GeminiRequest(
            Contents: messages.ToArray(),
            SystemInstruction: GeminiContent.CreateFromText(settings.SystemPrompt, GeminiContent.MessageRole.User),
            GenerationConfig: new GenConfig(settings.Temperature, thinkingConfig)
        );
    }

    public record GenConfig(decimal Temperature, ThinkingConfig? ThinkingConfig = null);

    public record ThinkingConfig(int ThinkingBudget)
    {
        public static ThinkingConfig? Create(ThinkingBudget? thinkingBudget)
        {
            if (thinkingBudget is null or Unknown)
                return null;

            if (thinkingBudget == Dynamic)
                return new ThinkingConfig(-1);

            var budget = thinkingBudget.Value.ToThinkingTokens();
            return new ThinkingConfig(budget);
        }
    }
}