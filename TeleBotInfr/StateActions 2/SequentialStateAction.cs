using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBotInfr.StateActions;

public class SequentialStateAction<TState, TStep> : StateAction<TState>
    where TState : SequentialState<TStep> where TStep : notnull
{
    private readonly Dictionary<TStep, IStepStateAction<TState, TStep>> stepToAction;

    public SequentialStateAction(IEnumerable<IStepStateAction<TState, TStep>> actions)
    {
        stepToAction = actions.ToDictionary(action => action.StateStep);
    }

    protected override async Task Execute(TState state, Message message, ITelegramBotClient bot, User user)
    {
        var currentStep = state.Current;
        if (!stepToAction.TryGetValue(currentStep, out var action))
            throw new KeyNotFoundException($"Unable to find action for step {currentStep}");

        var nextStep = await action.Execute(state, message, bot, user);
        if (nextStep)
            NextStepOrReset(state, user);
    }

    private static void NextStepOrReset(TState state, User user)
    {
        var hasNext = state.Next();
        if (hasNext && !state.IsCompleted)
            return;
            
        user.StateMachine.Reset();
    }
}