using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;
public abstract class SimpleSettingsPart<TState>(string name)
    : SettingsPart<TState>(name) where TState : SettingsState
{
    public override void SaveToState(TState state, UserState prevState)
    {
        throw new InvalidOperationException("Simple part result cannot be saved from previous state");
    }
}