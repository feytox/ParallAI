using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class StandardSettingsHandler<TState>(string tag, Action<SettingsPartsBuilder<TState>> partsProvider)
    : SettingsHandler<TState>(tag, partsProvider)
    where TState : SettingsState
{
    protected abstract string GetPartsMessage(TState state);
    protected abstract Task SaveSettingsToUser(TState state, ChatId chatId, ITelegramBotClient bot, User user);

    protected override async Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        var buttons = CreatePartButtons()
            .Chunk(2)
            .Append([InlineKeyboardButton.WithCallbackData("Сохранить", $"{Tag}:c")])
            .Append([CancelCallback.CreateButton("Выйти без сохранения")]);
        var message = GetPartsMessage(state);

        await bot.SendMessage(chatId, message, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    public override async Task FinalizeSettings(TState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        await SaveSettingsToUser(state, chatId, bot, user);
        user.StateMachine.Pop();
    }
}