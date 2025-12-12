using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
    : HttpGenHandler<OpenRouterProvider, OpenRouterRequest, OpenAiMessage, OpenAiResponse>(provider, model, client, logger)
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

    protected override async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var errorResponse = await response.Content.ReadFromJsonAsync<OpenRouterErrorResponse>(JsonSerializerOptions.Web);
        var message = $"Failed request to OpenRouter provider with Code: {errorResponse!.Error.Code}, " +
                      $"Message: {errorResponse.Error.Message}";
        var userMessage = $"Произошла ошибка при отправке запроса к модели {model.DisplayName} " +
                          $"провайдера OpenRouter c текстом {errorResponse.Error.Message}. ";
        if (errorResponse.Error.Code == 401)
            throw new UserFriendlyException(message, userMessage + "Введите корректный API ключ провайдера модели");
        throw new UserFriendlyException(message, userMessage);
    }
}