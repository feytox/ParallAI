using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class SequentialStateAction<TState, TStep> : StateAction<TState>
    where TState : SequentialState<TStep> where TStep : notnull
{
    private readonly Dictionary<TStep, IStepStateAction<TState, TStep>> stepToAction;
    private readonly bool endSilently;

    public SequentialStateAction(IEnumerable<IStepStateAction<TState, TStep>> actions, bool endSilently)
    {
        stepToAction = actions.ToDictionary(action => action.StateStep);
        this.endSilently = endSilently;
    }

    protected override async Task<ActionResult> Execute(TState state, Message message, 
        ITelegramBotClient bot, User user)
    {
        var currentStep = state.Current;
        if (!stepToAction.TryGetValue(currentStep, out var action))
            throw new KeyNotFoundException($"Unable to find action for step {currentStep}");

        var nextStep = await action.Execute(state, message, bot, user);
        if (nextStep)
            NextStepOrEnd(state, user);

        return ActionResult.Handled;
    }

    private void NextStepOrEnd(TState state, User user)
    {
        var hasNext = state.Next();
        if (hasNext && !state.IsCompleted)
            return;
            
        user.StateMachine.Pop(reactivate: !endSilently);
    }
}