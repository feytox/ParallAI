using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Util;

public static class MessageExtensions
{
    public static bool IsMediaGroup(this Message message)
    {
        return message.MediaGroupId is not null;
    }
}