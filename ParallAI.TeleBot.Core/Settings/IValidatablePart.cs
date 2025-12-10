using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;

public interface IValidatablePart<in TState> where TState : SettingsState
{
    bool Validate(TState state);
}