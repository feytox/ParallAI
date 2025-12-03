using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;

public interface IComplexSettingsPart<in TState> where TState : SettingsState
{
    public void SaveToState(TState state, UserState prevState);
}