using Infrastructure.Services;
using Infrastructure.ValueTypes;

namespace TeleBot.Services;

public class TgFileService : IFileService
{
    public async Task<AiFileInfo> DownloadFile(Stream stream)
    {
        throw new NotImplementedException();
    }
}