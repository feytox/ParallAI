using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class StandardSettingsPart<TState>(string name, string cancelTag, ParseMode parseMode = ParseMode.None): 
    SettingsPart<TState>(name) 
    where TState : SettingsState
{
    protected abstract string InputMessage { get; }
    
    protected abstract IEnumerable<InlineKeyboardButton>? GetInputButtons(User user);
    
    public override async Task<UserState?> ActivatePart(TState state, CallbackQuery query, ITelegramBotClient bot, User user)
    {
        var buttons = (GetInputButtons(user) ?? Enumerable.Empty<InlineKeyboardButton>())
            .Chunk(2)
            .Append([InlineKeyboardButton.WithCallbackData("Отмена", $"{cancelTag}")]);
        await bot.EditCallbackMessage(query, InputMessage, parseMode: parseMode, replyMarkup: new InlineKeyboardMarkup(buttons));
        return null;
    }
}