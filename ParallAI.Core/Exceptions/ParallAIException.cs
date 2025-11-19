namespace ParallAI.Core.Exceptions;

public class ParallAIException : Exception
{
    public ParallAIException()
    {
    }

    public ParallAIException(string message) : base(message)
    {
    }

    public ParallAIException(string message, Exception innerException) : base(message, innerException)
    {
    }
}