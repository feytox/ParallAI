using ParallAI.Core.States.Common;

namespace ParallAI.Core.States;

public class MainMenuState : UserState, IReactivatableState
{
    public bool Reactivated { get; set; } = true;
    
    public void AcceptPrevState(UserState? state) => Reactivated = true;
}