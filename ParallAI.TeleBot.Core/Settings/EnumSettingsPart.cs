using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public class EnumSettingsPart<TEnum, TState>(
    string tag,
    string cancelTag,
    string name,
    string inputMessage,
    Func<TEnum, bool> selector,
    Action<TState, TEnum> setter) 
    : StandardSettingsPart<TState>(name, cancelTag), ICallbackHandlerPart<TState>, IValidatablePart<TState>
    where TState : SettingsState 
    where TEnum : struct, Enum
{
    private readonly string[] enumNames = Enum.GetValues<TEnum>().Where(selector).Select(Enum.GetName).ToArray()!;

    protected override string InputMessage => inputMessage;

    protected override IEnumerable<InlineKeyboardButton>? InputButtons => enumNames
        .Select(name => InlineKeyboardButton.WithCallbackData(name, $"{tag}:{name}"));

    public Task<bool> HandleCallBack(TState state, CallbackQuery callback, ITelegramBotClient bot, User user)
    {
        var data = callback.Data!.Split(':');
        if (data.Length != 2)
            throw new FormatException("Invalid callback data");

        var value = Enum.Parse<TEnum>(data[1]);
        setter(state, value);
        return Task.FromResult(true);
    }

    public bool Validate(TState state) => true;
}