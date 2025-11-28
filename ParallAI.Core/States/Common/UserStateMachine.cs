namespace ParallAI.Core.States.Common;

public class UserStateMachine
{
    public UserState Current => States[^1];
    
    private List<UserState> States { get; set; } = [new MainMenuState()];

    public void Push(UserState state)
    {
        States.Add(state);
    }

    public void Pop(bool reactivate = true)
    {
        if (States.Count == 1)
            throw new InvalidOperationException("Unable to pop the default UserState.");

        var prevState = Current;
        States.RemoveAt(States.Count - 1);
        
        if (reactivate && Current is IReactivatableState state)
            state.AcceptPrevState(prevState);
    }
}