using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI API Reference</see>
/// </remarks>
public class OpenAiGenHandler(
    OpenAICompatibleProvider provider,
    AiModel model,
    HttpClient client,
    IFileService fileService,
    ILogger<OpenAiGenHandler>? logger = null)
    : HttpGenHandler<OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>(provider, model, client, logger)
{
    protected override Uri GetEndpointUrl()
    {
        return Provider.EndpointUrl;
    }

    protected override Task<OpenAiRequest> CreateTextRequest(TextMessage message, PromptSettings promptSettings)
    {
        var request = OpenAiRequest.Create(Model.ModelId, message, promptSettings);
        return Task.FromResult(request);
    }

    protected override async Task<OpenAiRequest> CreateFileRequest(FileMessage message, PromptSettings promptSettings)
    {
        var fileTasks = message.Files.Select(async info => await fileService.DownloadFile(info));
        var files = await Task.WhenAll(fileTasks);
        return OpenAiRequest.Create(Model.ModelId, message, files, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }
}