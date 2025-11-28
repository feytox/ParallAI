using ParallAI.Core.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class SequentialStateAction<TState, TStep> : StateAction<TState>
    where TState : SequentialState<TStep> where TStep : notnull
{
    private readonly Dictionary<TStep, IStepStateAction<TState, TStep>> stepToAction;

    public SequentialStateAction(IEnumerable<IStepStateAction<TState, TStep>> actions)
    {
        stepToAction = actions.ToDictionary(action => action.StateStep);
    }

    protected override async Task<bool> Execute(TState state, Message message, ITelegramBotClient bot, User user)
    {
        var currentStep = state.Current;
        if (!stepToAction.TryGetValue(currentStep, out var action))
            throw new KeyNotFoundException($"Unable to find action for step {currentStep}");

        var nextStep = await action.Execute(state, message, bot, user);
        if (nextStep)
            NextStepOrEnd(state, user);

        return true;
    }

    private static void NextStepOrEnd(TState state, User user)
    {
        var hasNext = state.Next();
        if (hasNext && !state.IsCompleted)
            return;
            
        user.StateMachine.Pop();
    }
}