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

    protected override OpenAiRequest CreateAiRequest(string modelId, Prompt prompt,
        PromptSettings promptSettings)
    {
        return OpenAiRequest.Create(modelId, prompt, promptSettings);
    }

    protected override void FillHttpRequest(OpenAICompatibleProvider provider, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.Token);
    }
}