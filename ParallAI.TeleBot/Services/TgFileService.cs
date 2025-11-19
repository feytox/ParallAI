using ParallAI.Core;
using ParallAI.Core.ValueTypes;
using Telegram.Bot;

namespace ParallAI.TeleBot.Services;

public class TgFileService(Bot bot) : IFileService
{
    public async Task DownloadFile(AiFileInfo fileInfo, Stream stream)
    {
        await bot.Client.GetInfoAndDownloadFile(fileInfo.FileId, stream);
    }
}