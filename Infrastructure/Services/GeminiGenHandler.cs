using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AICore.Entities;
using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public class GeminiGenHandler(
    GeminiProvider provider,
    AiModel model,
    HttpClient client,
    IFileService fileService,
    ILogger<GeminiGenHandler>? logger = null)
    : HttpGenHandler<GeminiProvider, GeminiRequest, GeminiResponse>(provider, model, client, logger)
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";
    private const string UploadUrl = "https://generativelanguage.googleapis.com/upload/v1beta/files";

    protected override Uri GetEndpointUrl()
    {
        return new Uri($"{BaseUrl}/{Model.ModelId}:generateContent");
    }

    protected override Task<GeminiRequest> CreateTextRequest(TextPrompt prompt, PromptSettings promptSettings)
    {
        var request = GeminiRequest.CreateText(prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override async Task<GeminiRequest> CreateFileRequest(FilePrompt prompt, PromptSettings promptSettings)
    {
        var fileUrlTasks = prompt.Files.Select(DownloadAndUploadFile);
        var fileUrls = await Task.WhenAll(fileUrlTasks);
        return GeminiRequest.CreateFile(prompt, fileUrls, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", Provider.Token);
    }

    private async Task<string> DownloadAndUploadFile(AiFileInfo fileInfo)
    {
        using var stream = new MemoryStream();
        await fileService.DownloadFile(fileInfo, stream);
        return await UploadFile(stream, fileInfo);
    }

    private async Task<string> UploadFile(Stream fileStream, AiFileInfo fileInfo)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, UploadUrl);
        using var formData = new MultipartFormDataContent();
        using var streamContent = new StreamContent(fileStream);

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(fileInfo.MimeType);
        formData.Add(streamContent, "file", fileInfo.FileId);

        request.Content = formData;
        FillHttpRequest(request);

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var file = await response.Content.ReadFromJsonAsync<GeminiFile>(JsonSerializerOptions.Web);
        return file!.Uri;
    }
}