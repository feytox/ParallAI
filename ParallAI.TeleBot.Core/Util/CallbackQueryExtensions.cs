using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Util;

public static class CallbackQueryExtensions
{
    public static Message GetMessage(this CallbackQuery query)
    {
        return query.Message ?? throw new NullReferenceException("Callback's message is null");
    }
    
    public static ChatId GetChatId(this CallbackQuery query)
    {
        ChatId? chatId = query.Message?.Chat;
        return chatId ?? query.From.Id;
    }
}