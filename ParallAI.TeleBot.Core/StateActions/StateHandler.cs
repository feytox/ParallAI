using ParallAI.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class StateHandler(IEnumerable<IStateAction> actions, IRepository<User, long> userRepository)
{
    public async Task<bool> HandleState(Message message, ITelegramBotClient bot)
    {
        var user = await userRepository.GetOrCreate(message.Chat.Id);
        var state = user.StateMachine.Current;

        var action = actions.FirstOrDefault(a => a.CanHandle(state));
        if (action == null)
            throw new KeyNotFoundException($"Unable to find action for state {state.GetType().Name}");

        var skipOtherHandling = await action.Execute(state, message, bot, user);
        await userRepository.Update(user);
        return skipOtherHandling;
    }
}
