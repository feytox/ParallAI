using System.Net.Http.Json;
using System.Text.Json;
using AICore.Entities;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

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
    
    protected abstract Task<TRequest> CreateTextRequest(TextPrompt prompt, PromptSettings promptSettings);

    protected abstract Task<TRequest> CreateFileRequest(FilePrompt prompt, PromptSettings promptSettings);

    protected abstract void FillHttpRequest(HttpRequestMessage request);
    
    public async Task<AiResponse> Generate(Prompt prompt, PromptSettings promptSettings)
    {
        var url = GetEndpointUrl();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var aiRequest = await CreateRequest(prompt, promptSettings);

        FillHttpRequest(request);
        request.Content = JsonContent.Create(aiRequest, options: JsonSerializerOptions.Web);

        logger?.LogInformation(JsonSerializer.Serialize(aiRequest, JsonSerializerOptions.Web));

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var providerResponse = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Web);
        return providerResponse!.ToTextResponse();
    }
    
    private async Task<TRequest> CreateRequest(Prompt prompt, PromptSettings promptSettings)
    {
        return prompt switch
        {
            FilePrompt filePrompt => await CreateFileRequest(filePrompt, promptSettings),
            TextPrompt textPrompt => await CreateTextRequest(textPrompt, promptSettings),
            _ => throw new ArgumentOutOfRangeException(nameof(prompt), prompt, null)
        };
    }
}