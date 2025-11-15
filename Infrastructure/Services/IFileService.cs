using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

public interface IFileService
{
    Task<AiFileInfo> DownloadFile(Stream stream);
}