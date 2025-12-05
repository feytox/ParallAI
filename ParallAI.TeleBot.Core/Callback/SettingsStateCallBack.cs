using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback;

public abstract class SettingsStateCallBack<TState>(IRepository<User, long> users, SettingsHandler<TState> handler)
    : UserCallbackQuery(users)
    where TState : SettingsState
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var currentState = user.StateMachine.Current;
        if (currentState is TState state)
            await handler.HandleCallBack(state, callbackQuery, bot, user);
    }
}
