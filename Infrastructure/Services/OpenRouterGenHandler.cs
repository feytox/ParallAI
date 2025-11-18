using System.Net.Http.Headers;
using AICore;
using AICore.Entities;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://openrouter.ai/docs/quickstart">OpenRouter API Reference</see>
/// </remarks>
public class OpenRouterGenHandler(
    OpenRouterProvider provider,
    AiModel model,
    HttpClient client,
    IFileService fileService,
    ILogger<OpenRouterGenHandler>? logger = null)
    : HttpGenHandler<OpenRouterProvider, OpenRouterRequest, OpenAiResponse>(provider, model, client, logger)
{
    private static readonly Uri BaseUrl = new("https://openrouter.ai/api/v1/chat/completions");

    protected override Uri GetEndpointUrl() => BaseUrl;

    protected override Task<OpenRouterRequest> CreateTextRequest(TextPrompt prompt, PromptSettings promptSettings)
    {
        var request = OpenRouterRequest.Create(Model.ModelId, prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override async Task<OpenRouterRequest> CreateFileRequest(FilePrompt prompt, PromptSettings promptSettings)
    {
        var fileTasks = prompt.Files.Select(async info => await fileService.DownloadFile(info));
        var files = await Task.WhenAll(fileTasks);
        return OpenRouterRequest.Create(Model.ModelId, prompt, files, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }
}