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
/// <see href="https://openrouter.ai/docs/quickstart">OpenRouter API Reference</see>
/// </remarks>
public class OpenRouterGenHandler(
    OpenRouterProvider provider,
    AiModel model,
    HttpClient client,
    IFileService fileService)
    : HttpGenHandler<OpenRouterProvider, OpenRouterRequest, OpenAiMessage, OpenAiResponse>(provider, model, client)
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

    protected override Task HandleErrorResponse(HttpResponseMessage response, string content)
    {
        var errorResponse = JsonSerializer.Deserialize<OpenRouterErrorResponse>(content, JsonSerializerOptions.Web);
        var message = $"Failed request to OpenRouter provider with Code: '{errorResponse!.Error.Code}', " +
                      $"Message: '{errorResponse.Error.Message}'";

        throw new GenerationException(message, errorResponse.Error.Message, "OpenRouter", Model.DisplayName);
    }
}