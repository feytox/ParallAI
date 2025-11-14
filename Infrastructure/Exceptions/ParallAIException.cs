namespace Infrastructure.Exceptions;

public class ParallAIException : Exception
{
    public ParallAIException() : base() {}
    public ParallAIException(string message) : base(message){}
    public ParallAIException(string message, Exception innerException) : base(message, innerException) {}
}