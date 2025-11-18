using AICore;
using AICore.ValueTypes;
using Telegram.Bot;

namespace TeleBot.Services;

public class TgFileService(Bot bot) : IFileService
{
    public async Task DownloadFile(AiFileInfo fileInfo, Stream stream)
    {
        await bot.Client.GetInfoAndDownloadFile(fileInfo.FileId, stream);
    }
}