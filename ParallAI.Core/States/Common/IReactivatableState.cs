namespace ParallAI.Core.States.Common;

public interface IReactivatableState : IPrevStateHandler
{
    public bool Reactivated { get; set; }
    
    void IPrevStateHandler.AcceptPrevState(UserState? state) => Reactivated = true;
}