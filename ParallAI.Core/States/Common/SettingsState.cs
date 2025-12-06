namespace ParallAI.Core.States.Common;

public abstract class SettingsState : UserState, IPrevStateHandler
{
    public int? CurrentPart { get; set; }
    public UserState? PrevState { get; private set; }
    public bool Reactivated { get; set; } = true;
    
    public void AcceptPrevState(UserState? state)
    {
        PrevState = state;
        Reactivated = true;
    }

    public void RejectPrevState() => PrevState = null;
}