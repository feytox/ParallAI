using AICore.ValueTypes;
using Infrastructure.ValueTypes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#method:-models.generatecontent">Gemini API Reference</see>
/// </remarks>
public class GeminiGenService(HttpClient client, IFileService fileService, ILogger<GeminiGenService>? logger = null)
    : HttpGenService<GeminiProvider, GeminiRequest, GeminiResponse>(client, logger)
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

    protected override Uri GetEndpointUrl(GeminiProvider provider, string modelId)
    {
        return new Uri($"{BaseUrl}/{modelId}:generateContent");
    }

    protected override Task<GeminiRequest> CreateTextRequest(GeminiProvider provider, string modelId, 
        TextPrompt prompt, PromptSettings promptSettings)
    {
        var request = GeminiRequest.CreateText(prompt, promptSettings);
        return Task.FromResult(request);
    }

    protected override async Task<GeminiRequest> CreateFileRequest(GeminiProvider provider, string modelId, 
        FilePrompt prompt, PromptSettings promptSettings)
    {
        var fileUrlTasks = prompt.FileIds.Select(DownloadAndUploadFile);
        var fileUrls = await Task.WhenAll(fileUrlTasks);
        return GeminiRequest.CreateFile(prompt, fileUrls, promptSettings);
    }

    protected override void FillHttpRequest(GeminiProvider provider, HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", provider.Token);
    }

    private async Task<Uri> DownloadAndUploadFile(string fileId)
    {
        using var stream = new MemoryStream();
        var info = await fileService.DownloadFile(stream);
        return await UploadFile(stream, info);
    }

    private async Task<Uri> UploadFile(Stream fileStream, AiFileInfo fileInfo)
    {
        throw new NotImplementedException(); // TODO
    }
}