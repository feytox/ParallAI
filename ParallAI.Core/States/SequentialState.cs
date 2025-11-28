using ParallAI.Core.States.Common;

namespace ParallAI.Core.States;

public abstract class SequentialState<TStep>(TStep[] steps) : UserState
{
    // ReSharper disable once MemberCanBePrivate.Global (used in serialization)
    public int Index { get; set; }
    public TStep Current => steps[Index];
    public override bool IsCompleted => Index >= steps.Length;

    public bool Next()
    {
        if (IsCompleted) 
            return false;
        Index++;
        return true;
    }
}