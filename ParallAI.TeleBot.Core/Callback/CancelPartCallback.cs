using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

public abstract class CancelPartCallback<TState, THandler>(IRepository<User, long> users, THandler handler)
    : UserCallbackQuery(users)
    where TState : SettingsState
    where THandler : SettingsHandler<TState>
{
    protected override async Task Handle(CallbackQuery query, ITelegramBotClient bot, User user)
    {
        var currentState = user.StateMachine.Current;
        if (currentState is not TState state)
            throw new InvalidOperationException($"{typeof(TState)} callback called for {currentState}");
        await handler.HandleCancelPart(state, query, bot, user);
    }
}