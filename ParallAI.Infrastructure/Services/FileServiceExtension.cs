using ParallAI.Core;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public static class FileServiceExtensions
{
    public static async Task<AiFile> DownloadFile(this IFileService service, AiFileInfo fileInfo)
    {
        using var stream = new MemoryStream();
        await service.DownloadFile(fileInfo, stream);
        return new AiFile(stream.ToArray(), fileInfo);
    }
}