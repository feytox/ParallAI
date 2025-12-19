using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ParallAI.Core.Entities;
using ParallAI.Core.Exceptions;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public abstract class HttpGenHandler<TProvider, TRequest, TMessage, TResponse>(
    TProvider provider,
    AiModel model,
    HttpClient client)
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

    protected abstract Task HandleErrorResponse(HttpResponseMessage response, string content);

    public async Task<AiResponse> Generate(AiMessage[] aiMessages, PromptSettings promptSettings,
        CancellationToken cancellationToken = default)
    {
        var url = GetEndpointUrl();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var messageTasks = aiMessages.Select(async m => await CreateMessage(m));
        var aiRequest = CreateRequest(await Task.WhenAll(messageTasks), promptSettings);

        FillHttpRequest(request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);

        var response = await Client.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            return await HandleResponse(response, content);
        }
        catch (NullReferenceException e)
        {
            throw CreateDeserializeException(content, e);
        }
        catch (JsonException e)
        {
            throw CreateDeserializeException(content, e);
        }
    }

    private async Task<AiResponse> HandleResponse(HttpResponseMessage response, string content)
    {
        if (IsClientError(response.StatusCode))
            await HandleErrorResponse(response, content);
        response.EnsureSuccessStatusCode();

        var providerResponse = JsonSerializer.Deserialize<TResponse>(content, JsonSerializerOptions.Web);
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

    private GenerationException CreateDeserializeException(string content, Exception innerException)
    {
        throw new GenerationException($"Unable to deserialize answer '{content}'", "Unable to deserialize answer", 
            typeof(TProvider).Name, model.DisplayName, innerException);
    }

    private static bool IsClientError(HttpStatusCode statusCode) => (int)statusCode >= 400 && (int)statusCode < 500;
}