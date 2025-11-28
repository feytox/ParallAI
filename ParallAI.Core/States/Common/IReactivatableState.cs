namespace ParallAI.Core.States.Common;

public interface IReactivatableState
{
    public void AcceptPrevState(UserState state);
}