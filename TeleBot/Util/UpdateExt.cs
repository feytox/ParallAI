using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBot.Util;

public static class UpdateExt
{
    public static ChatId? GetChatId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message!.Chat,
            UpdateType.CallbackQuery => update.CallbackQuery!.From.Id,
            _ => null
        };
    }
}