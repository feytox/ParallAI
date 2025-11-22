using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

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

    protected override Task<OpenRouterRequest> CreateTextRequest(TextMessage message, PromptSettings promptSettings)
    {
        var request = OpenRouterRequest.Create(Model.ModelId, message, promptSettings);
        return Task.FromResult(request);
    }

    protected override async Task<OpenRouterRequest> CreateFileRequest(FileMessage message, PromptSettings promptSettings)
    {
        var fileTasks = message.Files.Select(async info => await fileService.DownloadFile(info));
        var files = await Task.WhenAll(fileTasks);
        return OpenRouterRequest.Create(Model.ModelId, message, files, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }
}