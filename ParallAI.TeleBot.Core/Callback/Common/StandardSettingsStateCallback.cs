using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class StandardSettingsStateCallback<TState, THandler>(IRepository<User, long> users, THandler handler)
    : SettingsStateCallback<TState, THandler>(users, handler)
    where TState : SettingsState
    where THandler : StandardSettingsHandler<TState>
{
    protected override async Task<ActionResult> HandleDataContent(TState state, CallbackQuery query, string content,
        ITelegramBotClient bot, User user)
    {
        if (content != "c")
            return ActionResult.Skipped;

        await Handler.FinalizeSettings(state, query, bot, user);
        return ActionResult.Handled;
    }
}