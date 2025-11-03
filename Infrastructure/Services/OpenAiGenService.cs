using System.Net.Http.Headers;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

public class OpenAiGenService(HttpClient client)
    : HttpGenService<OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>(client)
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