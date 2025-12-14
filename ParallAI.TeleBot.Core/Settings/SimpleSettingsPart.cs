using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public class SimpleSettingsPart<TValue, TState>(
    string name,
    string cancelTag,
    string inputMessage,
    string failMessage,
    Func<string, TValue?> parser,
    Func<TState, TValue?> getter,
    Action<TState, TValue> setter
    ) : StandardSettingsPart<TState>(name, cancelTag), IMessageHandlerPart<TState>, IValidatablePart<TState>
    where TState : SettingsState
{
    protected override string InputMessage => inputMessage;
    
    protected override IEnumerable<InlineKeyboardButton>? GetInputButtons(User user) => null;
    
    public async Task<bool> HandleMessage(TState state, Message message, ITelegramBotClient bot)
    {
        if (!TryParse(message, out var value))
        {
            await bot.SendMessage(message.Chat, failMessage);
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