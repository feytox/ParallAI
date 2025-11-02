using AICore.Repositories;
using AICore.States;
using Microsoft.Extensions.Logging;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot;

public class StateHandler
{
    private readonly Dictionary<UserStateType, IStateAction> stateActions;
    private readonly ILogger<StateHandler> logger;

    public StateHandler(
        IEnumerable<IStateAction> actions,
        ILogger<StateHandler> logger)
    {
        stateActions = actions.ToDictionary(a => a.HandledState, a => a);
        this.logger = logger; 
    }

    public async Task<bool> HandleState(Message message, ITelegramBotClient bot, User user)
    {
        var state = user.StateMachine.Current;
        
        if (state == null || state.CurrentStep == UserStateType.Default)
            return false;
        
        if (!stateActions.TryGetValue(state.CurrentStep, out var action))
            return false;

        await action.Execute(message, user, bot);
        return true;
    }

}