using ParallAI.TeleBot.Util;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Services;

public class MediaGroupCollector
{
    public async Task<Message[]?> CollectMessages(Message message)
    {
        if (!message.IsMediaGroup())
            return [message];

        throw new NotImplementedException(); // TODO: issue #38
    }
}