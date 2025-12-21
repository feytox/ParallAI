using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public class SimpleSettingsPart<TValue, TState>(
    string name,
    string inputMessage,
    string failMessage,
    Func<string, TValue?> parser,
    Func<TState, TValue?> getter,
    Action<TState, TValue> setter
) : SettingsPart<TState>(name), IMessageHandlerPart<TState>, IValidatablePart<TState>
    where TState : SettingsState
{
    public override async Task<UserState?> ActivatePart(TState state, CallbackQuery query, ITelegramBotClient bot,
        User user)
    {
        var keyboard = new InlineKeyboardMarkup(PartBackCallback.Create());
        await bot.EditCallbackMessage(query, inputMessage, parseMode: ParseMode.Html, replyMarkup: keyboard);
        return null;
    }

    public async Task<bool> HandleMessage(TState state, Message message, ITelegramBotClient bot)
    {
        if (!TryParse(message, out var value))
        {
            var keyboard = new InlineKeyboardMarkup(PartBackCallback.Create());
            await bot.SendMessage(message.Chat, failMessage, parseMode: ParseMode.Html, replyMarkup: keyboard);
            return false;
        }

        setter(state, value);
        return true;
    }

    private bool TryParse(Message message, out TValue result)
    {
        result = default!;
        if (message.Text is null)
            return false;

        var value = parser(message.Text);
        if (value is null)
            return false;

        result = value;
        return true;
    }

    public bool Validate(TState state)
    {
        var value = getter(state);
        if (value is null)
            return false;

        return value is not string s || !string.IsNullOrWhiteSpace(s);
    }
}