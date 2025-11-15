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
        var fileUrlTasks = prompt.FileIds.Select(DownloadAndUploadFile);
        var fileUrls = await Task.WhenAll(fileUrlTasks);
        return GeminiRequest.CreateFile(prompt, fileUrls, promptSettings);
    }

    protected override void FillHttpRequest(HttpRequestMessage request)
    {
        request.Headers.Add("x-goog-api-key", Provider.Token);
    }

    private async Task<Uri> DownloadAndUploadFile(string fileId)
    {
        using var stream = new MemoryStream();
        var info = await fileService.DownloadFile(stream);
        return await UploadFile(stream, info);
    }

    private async Task<Uri> UploadFile(Stream fileStream, AiFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }
}