using AICore.States;

namespace AICore.Entities;

public class UserStateMachine
{
    public UserState? Current { get; private set; }
    
    public void Set(UserState state)
    {
        Current = state;
    }
    
    public void Reset()
    {
        Current = null;
    }
}