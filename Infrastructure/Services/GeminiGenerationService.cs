using System.Net.Http.Json;
using System.Text.Json;
using AICore.Services;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

public class GeminiGenerationService(HttpClient client) : IAiGenerationService<GeminiProvider>
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

    public async Task<AiResponse> Generate(GeminiProvider provider, string modelId,
        Prompt prompt, PromptSettings promptSettings)
    {
        var url = new Uri($"{BaseUrl}/{modelId}:generateContent");
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        var geminiRequest = GeminiRequest.Create(prompt, promptSettings);

        request.Headers.Add("x-goog-api-key", provider.Token);
        request.Content = JsonContent.Create(geminiRequest, options: JsonSerializerOptions.Web);

        var result = await client.SendAsync(request);
        var response = await result.Content.ReadFromJsonAsync<GeminiResponse>(JsonSerializerOptions.Web);
        return response!.ToTextResponse();
    }
}