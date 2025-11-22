using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public abstract class HttpGenHandler<TProvider, TRequest, TResponse>(
    TProvider provider,
    AiModel model,
    HttpClient client,
    ILogger<HttpGenHandler<TProvider, TRequest, TResponse>>? logger) 
    : IGenerationHandler
    where TProvider : AiProvider
    where TResponse : IGenResponse
{
    protected TProvider Provider { get; } = provider;
    protected AiModel Model { get; } = model;
    protected HttpClient Client { get; } = client;
    
    protected abstract Uri GetEndpointUrl();
    
    protected abstract Task<TRequest> CreateTextRequest(TextMessage message, PromptSettings promptSettings);

    protected abstract Task<TRequest> CreateFileRequest(FileMessage message, PromptSettings promptSettings);

    protected abstract void FillHttpRequest(HttpRequestMessage request);
    
    public async Task<AiResponse> Generate(AiMessage aiMessage, PromptSettings promptSettings)
    {
        var url = GetEndpointUrl();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var aiRequest = await CreateRequest(aiMessage, promptSettings);

        FillHttpRequest(request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);

        logger?.LogInformation(JsonSerializer.Serialize(aiRequest, JsonSerializerOptions.Web));

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var providerResponse = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
        return providerResponse!.ToTextResponse();
    }
    
    private async Task<TRequest> CreateRequest(AiMessage aiMessage, PromptSettings promptSettings)
    {
        return aiMessage switch
        {
            FileMessage filePrompt => await CreateFileRequest(filePrompt, promptSettings),
            TextMessage textPrompt => await CreateTextRequest(textPrompt, promptSettings),
            _ => throw new ArgumentOutOfRangeException(nameof(aiMessage), aiMessage, null)
        };
    }
}