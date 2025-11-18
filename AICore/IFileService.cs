using AICore.ValueTypes;

namespace AICore;

public interface IFileService
{
    Task DownloadFile(AiFileInfo fileInfo, Stream stream);
}