namespace AICore.States;

public abstract class SequentialState<TStep>(TStep[] steps) : UserState, ISequentialState
{
    public int index { get; private set; }
    public TStep Current => steps[index];
    public override bool IsCompleted => index >= steps.Length;

    public bool Next()
    {
        if (IsCompleted) return false;
        index++;
        return true;
    }
}