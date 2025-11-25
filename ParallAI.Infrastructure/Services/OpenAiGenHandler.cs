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
    : HttpGenHandler<OpenAICompatibleProvider, OpenAiRequest, OpenAiMessage, OpenAiResponse>(provider, model, client, logger)
{
    protected override Uri GetEndpointUrl()
    {
        return Provider.EndpointUrl;
    }

    protected override Task<OpenAiMessage> CreateTextMessage(TextMessage message)
    {
        var content = message.ToOpenAiMessage();
        return Task.FromResult(content);
    }

    protected override async Task<OpenAiMessage> CreateFileMessage(FileMessage message)
    {
        var fileTasks = message.Files.Select(async info => await fileService.DownloadFile(info));
        var files = await Task.WhenAll(fileTasks);
        return message.ToOpenAiMessage(files);
    }

    protected override OpenAiRequest CreateRequest(IEnumerable<OpenAiMessage> messages, PromptSettings promptSettings)
    {
        return OpenAiRequest.Create(Model.ModelId, messages, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }
}