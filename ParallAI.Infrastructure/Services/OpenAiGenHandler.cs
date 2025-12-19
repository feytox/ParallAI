using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
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
    IFileService fileService)
    : HttpGenHandler<OpenAICompatibleProvider, OpenAiRequest, OpenAiMessage, OpenAiResponse>(provider, model, client)
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

    protected override Task HandleErrorResponse(HttpResponseMessage response, string content)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new GenerationException(
                "Failed request to OpenAiCompatible provider with Code: 404 Not Found",
                "404 Not Found. Invalid Endpoint URL",
                "OpenAi Compatible",
                Model.DisplayName);

        var errorResponse = JsonSerializer.Deserialize<OpenAiErrorResponse>(content, JsonSerializerOptions.Web);
        var message = $"Failed request to OpenAiCompatible provider with Code: {errorResponse!.Error.Code}, " +
                      $"Message: {errorResponse.Error.Message}, Type: {errorResponse.Error.Type}, " +
                      $"Param: {errorResponse!.Error.Param}";

        throw new GenerationException(message, errorResponse.Error.Message, "OpenAi Compatible", Model.DisplayName);
    }
}