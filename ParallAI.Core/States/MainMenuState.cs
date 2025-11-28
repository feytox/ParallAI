using ParallAI.Core.States.Common;

namespace ParallAI.Core.States;

public class MainMenuState : UserState, IReactivatableState
{
    public override bool IsCompleted => false;
    public bool Reactivated { get; set; }
}