using AICore.States;

namespace AICore.Entities;

public class UserStateMachine
{
    public UserState? Current { get; private set; }
    
    public T? TryGetState<T>() where T : UserState
        => Current as T;

    public void Set(UserState state)
    {
        Current = state;
    }

    public void Reset()
    {
        Current = null;
    }

    public bool MoveNext()
    {
        if (Current == null) return false;

        var next = Current.Next();
        if (!next)
        {
            Reset();
            return false;
        }
        
        return true;
    }

    public bool IsActive => Current != null;
}