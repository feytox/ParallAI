#region

using AICore.Repositories;
using Microsoft.Extensions.Logging;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public class StateHandler(IEnumerable<IStateAction> actions, ILogger<StateHandler> logger, IRepository<User, long> userRepository)
{
    public async Task<bool> HandleState(Message message, ITelegramBotClient bot)
    {
        var user = await userRepository.GetOrCreate(message.Chat.Id);
        var state = user.StateMachine.Current;
        if (state == null) return false;

        var action = actions.FirstOrDefault(a => a.CanHandle(state));
        if (action == null)
        {
            logger.LogWarning("No action for state {State} for user {UserId}", state.GetType().Name, user.Id);
            return false;
        }

        await action.Execute(message, bot, user);
        await userRepository.Update(user);
        
        return true;
    }
}
