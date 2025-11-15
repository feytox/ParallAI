using System.Net.Http.Headers;
using AICore.Entities;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI API Reference</see>
/// </remarks>
public class OpenAiGenHandler(
    OpenAICompatibleProvider provider,
    AiModel model,
    HttpClient client,
    ILogger<OpenAiGenHandler>? logger = null)
    : HttpGenHandler<OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>(provider, model, client, logger)
{
    protected override Uri GetEndpointUrl()
    {
        return Provider.EndpointUrl;
    }

    protected override Task<OpenAiRequest> CreateTextRequest(TextPrompt prompt, PromptSettings promptSettings)
    {
        var request = OpenAiRequest.CreateText(Model.ModelId, prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override Task<OpenAiRequest> CreateFileRequest(FilePrompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }
}