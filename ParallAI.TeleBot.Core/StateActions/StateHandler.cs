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
        var mainSuccess = await ExecuteMainAction(message, bot, user);
        var afterSuccess = await ExecuteAfterAction(message, bot, user);
        
        await userRepository.Update(user);
        return mainSuccess || afterSuccess;
    }

    private async Task<bool> ExecuteMainAction(Message message, ITelegramBotClient bot, User user)
    {
        var state = user.StateMachine.Current;
        var action = GetCurrentAction(state);
        return await action.Execute(state, message, bot, user);
    }
    
    private async Task<bool> ExecuteAfterAction(Message message, ITelegramBotClient bot, User user)
    {
        var state = user.StateMachine.Current;
        var action = GetCurrentAction(state);
        return await action.ExecuteAfter(state, message, bot, user);
    }

    private IStateAction GetCurrentAction(UserState state)
    {
        var action = actions.FirstOrDefault(a => a.CanHandle(state));
        return action ?? throw new KeyNotFoundException($"Unable to find action for state {state.GetType().Name}");
    }
}
