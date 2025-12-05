using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;

public interface ICanSavePart<in TState> where TState : SettingsState
{
    void SaveToState(TState state, UserState prevState);
}

public interface ICanSavePart<in TState, in TPrevState> : ICanSavePart<TState>
    where TState : SettingsState where TPrevState : UserState
{
    void SaveToState(TState state, TPrevState prevState);

    void ICanSavePart<TState>.SaveToState(TState state, UserState prevUserState)
    {
        if (prevUserState is not TPrevState prevState)
            throw new ArgumentException($"{prevUserState.GetType()} state cannot be handled");
        SaveToState(state, prevState);
    }
}