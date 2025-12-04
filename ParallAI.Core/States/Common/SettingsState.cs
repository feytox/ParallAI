namespace ParallAI.Core.States.Common;

public abstract class SettingsState : UserState, IReactivatableState
{
    public int? CurrentPart { get; set; }
    public UserState? PrevState { get; private set; }
    public bool Reactivated { get; private set; }
    
    public void AcceptPrevState(UserState? state)
    {
        PrevState = state;
        Reactivated = true;
    }

    public void RejectPrevState()
    {
        PrevState = null;
        Reactivated = false;
    }
}