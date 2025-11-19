namespace ParallAI.Infrastructure;

public class Result<TError>
{
    private readonly TError? error;
    protected Result()
    {
        IsSuccess = true;
        error = default;
    }

    protected Result(TError error)
    {
        IsSuccess = false;
        this.error = error;
    }

    public bool IsSuccess { get; }
    
    public TError Error => !IsSuccess ? error! : throw new InvalidOperationException("Result is success");

    public static implicit operator Result<TError>(TError error) =>
        new(error);

    public static Result<TError> Success() =>
        new();

    public static Result<TError> Failure(TError error) =>
        new(error);
}

public class Result<TValue, TError> : Result<TError>
{
    private readonly TValue? value;
    private Result(TValue value) : base()
    {
        this.value = value;
    }

    private Result(TError error) : base(error)
    { 
        value = default;
    }

    public TValue Value => IsSuccess ? value! : throw new InvalidOperationException("Result is not success");

    public static implicit operator Result<TValue, TError>(TError error) =>
        new(error);

    public static implicit operator Result<TValue, TError>(TValue value) =>
        new(value);

    public static Result<TValue, TError> Success(TValue value) =>
        new(value);

    public new static Result<TValue, TError> Failure(TError error) =>
        new(error);
}