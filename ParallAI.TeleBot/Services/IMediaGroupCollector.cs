using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Services;

public interface IMediaGroupCollector
{
    public Task<Message[]?> CollectMessages(Message message);
}