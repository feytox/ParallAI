namespace ParallAI.Core.Exceptions;

public class GenerationException(
    string message,
    string apiMessage,
    string providerName,
    string modelName,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public string ApiMessage { get; } = apiMessage;
    public string ProviderName { get; } = providerName;
    public string ModelName { get; } = modelName;
}