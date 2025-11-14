namespace Infrastructure.Exceptions;

public sealed class UserCausedException : ParallAIException
{
    public string UserMessage { get; }

    public UserCausedException() : base()
    {
        UserMessage = Message;
    }

    public UserCausedException(string message, string userMessage) : base(message)
    {
        UserMessage = userMessage;
    }

    public UserCausedException(string message, string userMessage, Exception innerException) : base(message,
        innerException)
    {
        UserMessage = userMessage;
    }
}