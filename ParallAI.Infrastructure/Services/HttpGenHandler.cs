using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public abstract class HttpGenHandler<TProvider, TRequest, TMessage, TResponse>(
    TProvider provider,
    AiModel model,
    HttpClient client,
    ILogger<HttpGenHandler<TProvider, TRequest, TMessage, TResponse>>? logger) 
    : IGenerationHandler
    where TProvider : AiProvider
    where TResponse : IGenResponse
{
    protected TProvider Provider { get; } = provider;
    protected AiModel Model { get; } = model;
    protected HttpClient Client { get; } = client;
    
    protected abstract Uri GetEndpointUrl();
    
    protected abstract Task<TMessage> CreateTextMessage(TextMessage message);

    protected abstract Task<TMessage> CreateFileMessage(FileMessage message);
    
    protected abstract TRequest CreateRequest(IEnumerable<TMessage> messages, PromptSettings promptSettings);

    protected abstract void FillHttpRequest(HttpRequestMessage request);
    
    public async Task<AiResponse> Generate(AiMessage[] aiMessages, PromptSettings promptSettings)
    {
        var url = GetEndpointUrl();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var messageTasks = aiMessages.Select(async m => await CreateMessage(m));
        var aiRequest = CreateRequest(await Task.WhenAll(messageTasks), promptSettings);

        FillHttpRequest(request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);

        logger?.LogInformation(JsonSerializer.Serialize(aiRequest, JsonSerializerOptions.Web));

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var providerResponse = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
        return providerResponse!.ToTextResponse();
    }
    
    private async Task<TMessage> CreateMessage(AiMessage aiMessage)
    {
        return aiMessage switch
        {
            FileMessage filePrompt => await CreateFileMessage(filePrompt),
            TextMessage textPrompt => await CreateTextMessage(textPrompt),
            _ => throw new ArgumentOutOfRangeException(nameof(aiMessage), aiMessage, null)
        };
    }
}