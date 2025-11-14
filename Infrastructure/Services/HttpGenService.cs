using System.Net.Http.Json;
using System.Text.Json;
using AICore.Services;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public abstract class HttpGenService<TProvider, TRequest, TResponse>(
    HttpClient client,
    ILogger<HttpGenService<TProvider, TRequest, TResponse>>? logger)
    : IProviderGenService<TProvider> where TProvider : AiProvider where TResponse : IGenResponse
{
    protected abstract Uri GetEndpointUrl(TProvider provider, string modelId);
    protected abstract TRequest CreateAiRequest(string modelId, Prompt prompt, PromptSettings promptSettings);
    protected abstract void FillHttpRequest(TProvider provider, HttpRequestMessage request);

    public async Task<AiResponse> Generate(TProvider provider, string modelId,
        Prompt prompt, PromptSettings promptSettings)
    {
        var url = GetEndpointUrl(provider, modelId);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var aiRequest = CreateAiRequest(modelId, prompt, promptSettings);

        FillHttpRequest(provider, request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);
        
        logger?.LogInformation(JsonSerializer.Serialize(aiRequest, JsonSerializerOptions.Web));

        var result = await client.SendAsync(request);
        result.EnsureSuccessStatusCode();

        var response = await result.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
        return response!.ToTextResponse();
    }
}