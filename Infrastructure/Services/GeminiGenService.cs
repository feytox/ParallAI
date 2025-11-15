using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public class GeminiGenService(HttpClient client, ILogger<GeminiGenService>? logger = null)
    : HttpGenService<GeminiProvider, GeminiRequest, GeminiResponse>(client, logger)
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

    protected override Uri GetEndpointUrl(GeminiProvider provider, string modelId)
    {
        return new Uri($"{BaseUrl}/{modelId}:generateContent");
    }

    protected override async Task<GeminiRequest> CreateAiRequest(string modelId, Prompt prompt,
        PromptSettings promptSettings)
    {
        if (prompt is TextPrompt textPrompt)
            return GeminiRequest.CreateText(textPrompt, promptSettings);
        throw new ArgumentException();
    }

    protected override void FillHttpRequest(GeminiProvider provider, HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", provider.Token);
    }
}