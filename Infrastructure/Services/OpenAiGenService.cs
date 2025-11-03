using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AICore.Services;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

// TODO: deduplicate with GeminiGenService ???
public class OpenAiGenService(HttpClient client) : IProviderGenService<OpenAICompatibleProvider>
{
    public async Task<AiResponse> Generate(OpenAICompatibleProvider provider, string modelId, 
        Prompt prompt, PromptSettings promptSettings)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, provider.EndpointUrl);
        var openAiRequest = OpenAiRequest.Create(modelId, prompt, promptSettings);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.Token);
        request.Content = JsonContent.Create(openAiRequest, options: JsonSerializerOptions.Web);
        
        var result = await client.SendAsync(request);
        result.EnsureSuccessStatusCode();
        
        var response = await result.Content.ReadFromJsonAsync<OpenAiResponse>(JsonSerializerOptions.Web);
        return response!.ToTextResponse();
    }
}