using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public abstract class StateAction<TState> : IStateAction where TState : UserState
{
    protected abstract Task<ActionResult> Execute(TState state, Message message, ITelegramBotClient bot, User user);

    protected virtual Task<ActionResult> ExecuteAfter(TState state, ChatId chatId, ITelegramBotClient bot, User user)
        => Task.FromResult(ActionResult.Skipped);

    public Task<ActionResult> ExecuteAfter(UserState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        return ExecuteAfter((TState)state, chatId, bot, user);
    }

    public bool CanHandle(UserState? state) => state is TState;

    public Task<ActionResult> Execute(UserState state, Message message, ITelegramBotClient bot, User user)
    {
        return Execute((TState)state, message, bot, user);
    }
}