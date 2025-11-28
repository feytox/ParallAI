using ParallAI.Core.States.Common;

namespace ParallAI.Core.States;

public abstract class SettingsState : UserState, IReactivatableState
{
    public int? CurrentPart { get; set; }
    public UserState? PrevState { get; set; }
    
    public void AcceptPrevState(UserState state) => PrevState = state;
}