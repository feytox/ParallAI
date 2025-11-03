using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

public class GeminiGenService(HttpClient client) : HttpGenService<GeminiProvider, GeminiRequest, GeminiResponse>(client)
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

    protected override Uri GetEndpointUrl(GeminiProvider provider, string modelId)
    {
        return new Uri($"{BaseUrl}/{modelId}:generateContent");
    }

    protected override GeminiRequest CreateAiRequest(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        return GeminiRequest.Create(prompt, promptSettings);
    }

    protected override void FillHttpRequest(GeminiProvider provider, HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", provider.Token);
    }
}