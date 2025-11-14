using System.Net.Http.Headers;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://openrouter.ai/docs/quickstart">OpenRouter API Reference</see>
/// </remarks>
public class OpenRouterGenService(HttpClient client, ILogger<OpenRouterGenService>? logger = null)
    : HttpGenService<OpenRouterProvider, OpenRouterRequest, OpenAiResponse>(client, logger)
{
    private static readonly Uri BaseUrl = new("https://openrouter.ai/api/v1/chat/completions");
    
    protected override Uri GetEndpointUrl(OpenRouterProvider provider, string modelId) => BaseUrl;

    protected override Task<OpenRouterRequest> CreateAiRequest(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        var request = OpenRouterRequest.Create(modelId, prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override void FillHttpRequest(OpenRouterProvider provider, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.Token);
    }
}