namespace ParallAI.Core.States.Common;

public class UserStateMachine
{
    public UserState Current => States[^1];
    
    private List<UserState> States { get; set; } = [new MainMenuState()];

    public void Push(UserState state)
    {
        States.Add(state);
    }

    public void Pop(bool reactivate = true, bool clearPrevious = false)
    {
        if (!TryPop(reactivate, clearPrevious))
            throw new InvalidOperationException("Unable to pop the default UserState.");
    }

    public bool TryPop(bool reactivate = true, bool clearPrevious = false)
    {
        if (States.Count == 1)
            return false;

        var prevState = clearPrevious ? null : Current;
        States.RemoveAt(States.Count - 1);

        if (reactivate && Current is IReactivatableState state)
            state.AcceptPrevState(prevState);
        return true;
    }
}