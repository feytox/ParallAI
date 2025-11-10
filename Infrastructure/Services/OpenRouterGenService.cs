using System.Net.Http.Headers;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://openrouter.ai/docs/quickstart">OpenRouter API Reference</see>
/// </remarks>
public class OpenRouterGenService(HttpClient client)
    : HttpGenService<OpenRouterProvider, OpenRouterRequest, OpenAiResponse>(client)
{
    private static readonly Uri BaseUrl = new("https://openrouter.ai/api/v1/chat/completions");
    
    protected override Uri GetEndpointUrl(OpenRouterProvider provider, string modelId) => BaseUrl;

    protected override OpenRouterRequest CreateAiRequest(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        return OpenRouterRequest.Create(modelId, prompt, promptSettings);
    }

    protected override void FillHttpRequest(OpenRouterProvider provider, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.Token);
    }
}