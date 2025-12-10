using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Exceptions;
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
    : HttpGenHandler<OpenRouterProvider, OpenRouterRequest, OpenAiMessage, OpenAiResponse, OpenRouterErrorResponse>(provider, model, client, logger)
{
    private static readonly Uri BaseUrl = new("https://openrouter.ai/api/v1/chat/completions");

    protected override Uri GetEndpointUrl() => BaseUrl;
    
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

    protected override OpenRouterRequest CreateRequest(IEnumerable<OpenAiMessage> messages, PromptSettings promptSettings)
    {
        return OpenRouterRequest.Create(Model.ModelId, messages, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Provider.Token);
    }

    protected override void HandleErrorResponse(OpenRouterErrorResponse response)
    {
        throw new UserFriendlyException($"Failed request to OpenRouter provider with Code: {response.Error.Code}, " +
                                        $"Message: {response.Error.Message}",
            $"Произошла ошибка при отправке запроса к модели {model.DisplayName} " +
            $"провайдера OpenRouter c текстом {response.Error.Message}");
    }
}