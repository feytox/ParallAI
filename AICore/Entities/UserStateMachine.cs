namespace AICore.States;

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
    
    public bool NextStepOrNothing()
    {
        if (Current is null)
            return false;

        switch (Current)
        {
            case ISequentialState seq:
                var hasNext = seq.Next();
                if (!hasNext || seq.IsCompleted)
                    Reset();
                return hasNext;

            default:
                Reset();
                return false;
        }
    }
    
    public T? TryGetState<T>() where T : UserState
        => Current as T;
}