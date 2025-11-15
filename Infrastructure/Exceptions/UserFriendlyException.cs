namespace Infrastructure.Exceptions;

public class UserFriendlyException : ParallAIException
{
    public string UserMessage { get; }

    public UserFriendlyException() : base()
    {
        UserMessage = Message;
    }

    public UserFriendlyException(string message, string userMessage) : base(message)
    {
        UserMessage = userMessage;
    }

    public UserFriendlyException(string message, string userMessage, Exception innerException) : base(message,
        innerException)
    {
        UserMessage = userMessage;
    }
}