namespace ParallAI.Core.States.Common;

public interface IPrevStateHandler
{
    public void AcceptPrevState(UserState? state);
}