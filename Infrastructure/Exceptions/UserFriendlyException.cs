namespace Infrastructure.Exceptions;

public class UserFriendlyException : ParallAIException
{
    public string UserMessage => userMessage ?? Message;

    private readonly string? userMessage;

    public UserFriendlyException(string message, string userMessage) : base(message)
    {
        this.userMessage = userMessage;
    }

    public UserFriendlyException(string message, string userMessage, Exception innerException)
        : base(message, innerException)
    {
        this.userMessage = userMessage;
    }
}