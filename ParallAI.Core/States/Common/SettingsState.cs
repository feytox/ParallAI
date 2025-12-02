namespace ParallAI.Core.States.Common;

public abstract class SettingsState : UserState, IReactivatableState
{
    public int? CurrentPart { get; set; }
    public UserState? PrevState { get; set; }
    
    public void AcceptPrevState(UserState state) => PrevState = state;
    
    public void RejectPrevState() => PrevState = null;
}