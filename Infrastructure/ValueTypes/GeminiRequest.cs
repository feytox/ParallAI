using System.Text.Json.Serialization;
using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public record GeminiRequest(
    GeminiContent[] Contents,
    [property: JsonPropertyName("system_instruction")]
    GeminiContent SystemInstruction,
    GeminiRequest.GenConfig GenerationConfig)
{
    public static GeminiRequest CreateText(TextPrompt prompt, PromptSettings settings)
    {
        return new GeminiRequest(
            Contents: GeminiContent.CreateFromPrompt(prompt),
            SystemInstruction: GeminiContent.CreateFromText(settings.SystemInstructions),
            GenerationConfig: new GenConfig(settings.Temperature)
        );
    }

    // TODO: add image support
    public record GenConfig(double Temperature, ThinkingConfig? ThinkingConfig = null);

    public record ThinkingConfig(int ThinkingBudget);
}