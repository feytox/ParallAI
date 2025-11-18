using AICore;
using AICore.ValueTypes;

namespace Infrastructure.Services;

public static class FileServiceExt
{
    public static async Task<AiFile> DownloadFile(this IFileService service, AiFileInfo fileInfo)
    {
        using var stream = new MemoryStream();
        await service.DownloadFile(fileInfo, stream);
        return new AiFile(stream.ToArray(), fileInfo);
    }
}