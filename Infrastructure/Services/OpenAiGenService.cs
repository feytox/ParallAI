using System.Net.Http.Headers;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI API Reference</see>
/// </remarks>
public class OpenAiGenService(HttpClient client, ILogger<OpenAiGenService>? logger = null)
    : HttpGenService<OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>(client, logger)
{
    protected override Uri GetEndpointUrl(OpenAICompatibleProvider provider, string modelId)
    {
        return provider.EndpointUrl;
    }

    protected override Task<OpenAiRequest> CreateTextRequest(OpenAICompatibleProvider provider, string modelId, 
        TextPrompt prompt, PromptSettings promptSettings)
    {
        var request = OpenAiRequest.CreateText(modelId, prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override Task<OpenAiRequest> CreateFileRequest(OpenAICompatibleProvider provider, string modelId, 
        FilePrompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException(); // TODO
    }

    protected override void FillHttpRequest(OpenAICompatibleProvider provider, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.Token);
    }
}