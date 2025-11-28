using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public abstract class StateAction<TState> : IStateAction where TState : UserState
{
    protected abstract Task<bool> Execute(TState state, Message message, ITelegramBotClient bot, User user);

    public bool CanHandle(UserState? state) => state is TState;

    public Task<bool> Execute(UserState state, Message message, ITelegramBotClient bot, User user)
    {
        return Execute((TState)state, message, bot, user);
    }
}