namespace Infrastructure;

public class Result<TError>
{
    protected Result()
    {
        IsSuccess = true;
        Error = default;
    }

    protected Result(TError error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }
    
    public TError? Error { get; }

    public static implicit operator Result<TError>(TError error) =>
        new(error);

    public static Result<TError> Success() =>
        new();

    public static Result<TError> Failure(TError error) =>
        new(error);
}

public class Result<TValue, TError> : Result<TError>
{
    private Result(TValue value) : base()
    {
        Value = value;
    }

    private Result(TError error) : base(error)
    { 
        Value = default;
    }

    public TValue? Value { get; }

    public static implicit operator Result<TValue, TError>(TError error) =>
        new(error);

    public static implicit operator Result<TValue, TError>(TValue value) =>
        new(value);

    public static Result<TValue, TError> Success(TValue value) =>
        new(value);

    public new static Result<TValue, TError> Failure(TError error) =>
        new(error);
}