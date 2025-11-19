using ParallAI.Core.ValueTypes;

namespace ParallAI.Core;

public interface IFileService
{
    Task DownloadFile(AiFileInfo fileInfo, Stream stream);
}