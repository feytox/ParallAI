namespace ParallAI.Core.States.Common;

public class UserStateMachine
{
    public UserState Current => States.Peek();
    
    private Stack<UserState> States { get; set; } = new([new MainMenuState()]);

    public void Push(UserState state)
    {
        States.Push(state);
    }

    public void Pop()
    {
        if (States.Count == 1)
            throw new InvalidOperationException("Unable to pop the default UserState."); 
        
        States.Pop();
        
        if (Current is IReactivatableState state)
            state.Reactivated = true;
    }
}