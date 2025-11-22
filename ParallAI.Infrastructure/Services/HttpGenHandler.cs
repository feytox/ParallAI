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
    
    protected abstract Task<TMessage> CreateTextMessage(TextMessage message, PromptSettings promptSettings);

    protected abstract Task<TMessage> CreateFileMessage(FileMessage message, PromptSettings promptSettings);
    
    protected abstract Task<TRequest> CreateRequest(IEnumerable<TMessage> messages);

    protected abstract void FillHttpRequest(HttpRequestMessage request);
    
    public async Task<AiResponse> Generate(AiMessage[] aiMessages, PromptSettings promptSettings)
    {
        var url = GetEndpointUrl();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var messageTasks = aiMessages.Select(m => CreateMessage(m, promptSettings));
        var aiRequest = await CreateRequest(await Task.WhenAll(messageTasks));

        FillHttpRequest(request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);

        logger?.LogInformation(JsonSerializer.Serialize(aiRequest, JsonSerializerOptions.Web));

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var providerResponse = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
        return providerResponse!.ToTextResponse();
    }
    
    private async Task<TMessage> CreateMessage(AiMessage aiMessage, PromptSettings promptSettings)
    {
        return aiMessage switch
        {
            FileMessage filePrompt => await CreateFileMessage(filePrompt, promptSettings),
            TextMessage textPrompt => await CreateTextMessage(textPrompt, promptSettings),
            _ => throw new ArgumentOutOfRangeException(nameof(aiMessage), aiMessage, null)
        };
    }
}