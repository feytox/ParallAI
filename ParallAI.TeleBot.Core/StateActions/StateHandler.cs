using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class StateHandler(IEnumerable<IStateAction> actions, IRepository<User, long> userRepository)
{
    public async Task<bool> HandleState(Message message, ITelegramBotClient bot)
    {
        var user = await userRepository.GetOrCreate(message.From!.Id);
        return await Execute(user, async (state, action) => await action.Execute(state, message, bot, user));
    }

    public async Task<bool> HandlePostState(long userId, ITelegramBotClient bot)
    {
        var user = await userRepository.GetOrCreate(userId);
        return await Execute(user, async (state, action) => await action.ExecuteAfter(state, userId, bot, user));
    }

    private async Task<T> Execute<T>(User user, Func<UserState, IStateAction, Task<T>> executor)
    {
        var state = user.StateMachine.Current;
        var action = GetCurrentAction(state);
        var result = await executor(state, action);
        await userRepository.Update(user);
        return result;
    }

    private IStateAction GetCurrentAction(UserState state)
    {
        var action = actions.FirstOrDefault(a => a.CanHandle(state));
        return action ?? throw new KeyNotFoundException($"Unable to find action for state {state.GetType().Name}");
    }
}