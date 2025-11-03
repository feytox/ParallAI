namespace AICore.States;

public interface ISequentialState
{
    bool Next();
    bool IsCompleted { get; }
}