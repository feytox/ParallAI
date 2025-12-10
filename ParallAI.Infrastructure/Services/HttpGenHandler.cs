using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ParallAI.Core.Entities;
using ParallAI.Core.Exceptions;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public abstract class HttpGenHandler<TProvider, TRequest, TMessage, TResponse, TErrorResponse>(
    TProvider provider,
    AiModel model,
    HttpClient client,
    ILogger<HttpGenHandler<TProvider, TRequest, TMessage, TResponse, TErrorResponse>>? logger) 
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
    
    protected abstract void HandleErrorResponse(TErrorResponse response);
    
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
        //try catch нужен потому что какой-ни будь OpenRouter может прислать ответ с кодом 200, но содержащий ошибку в теле.
        //Try аналога метода десериализации не нашел :(
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<TErrorResponse>(JsonSerializerOptions.Web);
                HandleErrorResponse(errorResponse);
                response.EnsureSuccessStatusCode();
            }

            var providerResponse = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
            return providerResponse!.ToTextResponse();
        }
        catch (JsonException e)
        {
            throw new UserFriendlyException(
                $"Can't deserialize answer {await response.Content.ReadAsStringAsync()} from {typeof(TProvider).Name}",
                $"Произошла ошибка при отправке запроса к модели {model.DisplayName} провайдера {typeof(TProvider).Name}");
        }
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