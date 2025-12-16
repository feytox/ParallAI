using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback;

[CallbackQuery(Tag)]
public class PartBackCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    private const string Tag = "part_back";

    protected override Task Handle(CallbackQuery query, ITelegramBotClient bot, User user)
    {
        if (user.StateMachine.Current is SettingsState settingsState)
            settingsState.Reactivated = true;

        return Task.CompletedTask;
    }

    public static InlineKeyboardButton Create(string text = "🔙 Назад")
        => InlineKeyboardButton.WithCallbackData(text, Tag);
}
