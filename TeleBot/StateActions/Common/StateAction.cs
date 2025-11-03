#region

using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot.StateActions.Common;

public abstract class StateAction<TState> : IStateAction where TState : UserState
{
    protected abstract Task Execute(TState state, Message message, ITelegramBotClient bot, User user);

    public bool CanHandle(UserState? state) => state is TState;

    public Task Execute(UserState state, Message message, ITelegramBotClient bot, User user)
    {
        return Execute((TState)state, message, bot, user);
    }
}