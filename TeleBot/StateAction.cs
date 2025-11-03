#region

using AICore.Repositories;
using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot;

public abstract class StateAction<TState> : IStateAction where TState : UserState
{
    public virtual bool CanHandle(UserState? state)
    {
        return state is TState;
    }

    protected TState GetState(UserState state) => (TState)state;
    
    public abstract Task Execute(Message message, ITelegramBotClient bot, User user);
}