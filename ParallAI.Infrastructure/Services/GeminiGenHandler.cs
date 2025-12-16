using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Exceptions;
using ParallAI.Core.Providers;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.ValueTypes;

namespace ParallAI.Infrastructure.Services;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public class GeminiGenHandler(
    GeminiProvider provider,
    AiModel model,
    HttpClient client,
    IFileService fileService)
    : HttpGenHandler<GeminiProvider, GeminiRequest, GeminiContent, GeminiResponse>(provider, model, client)
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";
    private const string UploadUrl = "https://generativelanguage.googleapis.com/upload/v1beta/files";

    protected override Uri GetEndpointUrl()
    {
        return new Uri($"{BaseUrl}/{Model.ModelId}:generateContent");
    }

    protected override Task<GeminiContent> CreateTextMessage(TextMessage message)
    {
        var content = message.ToGeminiContent();
        return Task.FromResult(content);
    }

    protected override async Task<GeminiContent> CreateFileMessage(FileMessage message)
    {
        var fileUrlTasks = message.Files.Select(async info => await DownloadAndUploadFile(info));
        var fileUrls = await Task.WhenAll(fileUrlTasks);
        return message.ToGeminiContent(fileUrls);
    }

    protected override GeminiRequest CreateRequest(IEnumerable<GeminiContent> messages, PromptSettings promptSettings)
    {
        return GeminiRequest.Create(messages, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", Provider.Token);
    }

    protected override async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var errorResponse = await response.Content.ReadFromJsonAsync<GeminiErrorResponse>(JsonSerializerOptions.Web);
        var message = $"Failed request to Gemini provider with Code: {errorResponse!.Error.Code}, " +
                      $"Status: {errorResponse.Error.Status}, Message: {errorResponse.Error.Message}";
        var userMessage = $"Произошла ошибка при отправке запроса к модели '{Model.DisplayName}' " +
                          $"провайдера Gemini c текстом:\n'{errorResponse.Error.Message}'";
        throw new UserFriendlyException(message, userMessage);
    }

    private async Task<string> DownloadAndUploadFile(AiFileInfo fileInfo)
    {
        var file = await fileService.DownloadFile(fileInfo);
        return await UploadFile(file);
    }

    private async Task<string> UploadFile(AiFile file)
    {
        var uploadUrl = await StartUploading(file.Info, file.Content.Length);
        using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl);

        request.Headers.Add("X-Goog-Upload-Offset", "0");
        request.Headers.Add("X-Goog-Upload-Command", "upload, finalize");

        request.Content = new ByteArrayContent(file.Content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(file.Info.MimeType);

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(responseStream);
        return json.RootElement.GetProperty("file").GetProperty("uri").GetString()!;
    }

    private async Task<string> StartUploading(AiFileInfo fileInfo, long fileSize)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{UploadUrl}?key={Provider.Token}");
        var mimeType = ToSupportedMimeType(fileInfo.MimeType);

        request.Headers.Add("X-Goog-Upload-Protocol", "resumable");
        request.Headers.Add("X-Goog-Upload-Command", "start");
        request.Headers.Add("X-Goog-Upload-Header-Content-Length", fileSize.ToString());
        request.Headers.Add("X-Goog-Upload-Header-Content-Type", mimeType);
        request.Content = JsonContent.Create(new { file = new { display_name = fileInfo.FileId } });

        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response.Headers.GetValues("x-goog-upload-url").First();
    }

    private static string ToSupportedMimeType(string mimeType)
    {
        if (mimeType.StartsWith("text"))
            return "text/plain";

        return mimeType;
    }
}