using Infrastructure.ValueTypes;

namespace Infrastructure.Services;

public interface IFileService
{
    Task DownloadFile(AiFileInfo fileInfo, Stream stream);
}