using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ParallAI.TeleBot.Util;

public static class UpdateExtensions
{
    public static ChatId? GetChatId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message!.Chat,
            UpdateType.CallbackQuery => update.CallbackQuery!.Message?.Chat,
            _ => null
        };
    }
}