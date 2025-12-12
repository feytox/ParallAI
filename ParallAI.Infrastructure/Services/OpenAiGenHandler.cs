using System.Net;
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

    protected override async Task HandleErrorResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new UserFriendlyException("Failed request to OpenAiCompatible provider with Code: 404 Not Found",
                $"Произошла ошибка при отправке запроса к модели {model.DisplayName} " +
                $"провайдера OpenAiCompatible c текстом Not Found. Введите корректный Endpoint Url в параметрах модели");
        
        var errorResponse = await response.Content.ReadFromJsonAsync<OpenAiErrorResponse>(JsonSerializerOptions.Web);
        var message = $"Failed request to OpenAiCompatible provider with Code: {errorResponse!.Error.Code}, " +
                      $"Message: {errorResponse.Error.Message}, Type: {errorResponse.Error.Type}, " +
                      $"Param: {errorResponse!.Error.Param}";
        var userMessage = $"Произошла ошибка при отправке запроса к модели {model.DisplayName} " +
                          $"провайдера OpenAiCompatible c текстом {errorResponse.Error.Message}";
        throw new UserFriendlyException(message, userMessage);
    }
}