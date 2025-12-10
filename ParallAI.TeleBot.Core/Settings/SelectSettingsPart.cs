using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public class SelectSettingsPart<TState, TValue>(
    string tag,
    string name,
    string inputMessage,
    Func<User, IReadOnlyList<TValue>> elementsProvider,
    Func<TValue, string> nameSelector,
    Func<TState, TValue?> getter,
    Action<TState, TValue> setter)
    : SettingsPart<TState>(name), ICallbackHandlerPart<TState>, IValidatablePart<TState>
    where TState : SettingsState
{
    public override async Task<UserState?> ActivatePart(TState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        var buttons = elementsProvider(user)
            .Select(nameSelector)
            .Select((name, i) => InlineKeyboardButton.WithCallbackData(name, $"{tag}:{i}"))
            .Chunk(2);
        
        await bot.SendMessage(chatId, inputMessage, replyMarkup: new InlineKeyboardMarkup(buttons));
        return null;
    }

    public Task<bool> HandleCallBack(TState state, CallbackQuery callback, ITelegramBotClient bot, User user)
    {
        var content = callback.Data!.Split(':')[1];
        var index = int.Parse(content);
        var elements = elementsProvider(user);
        var selectedValue = elements[index];

        setter(state, selectedValue);
        return Task.FromResult(true);
    }

    public bool Validate(TState state)
    {
        var value = getter(state);
        return value is not null;
    }
}