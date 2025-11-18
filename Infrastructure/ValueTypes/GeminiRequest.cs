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
            Contents: GeminiContent.Create(prompt),
            SystemInstruction: GeminiContent.CreateFromText(settings.SystemPrompt),
            GenerationConfig: new GenConfig(settings.Temperature, new ThinkingConfig(settings.ThinkingBudget))
        );
    }

    public static GeminiRequest CreateFile(FilePrompt prompt, IEnumerable<string> fileUrls, PromptSettings settings)
    {
        return new GeminiRequest(
            Contents: GeminiContent.Create(prompt, fileUrls),
            SystemInstruction: GeminiContent.CreateFromText(settings.SystemPrompt),
            GenerationConfig: new GenConfig(settings.Temperature)
        );
    }
    
    public record GenConfig(double Temperature, ThinkingConfig? ThinkingConfig = null);

    public record ThinkingConfig(int ThinkingBudget);
}