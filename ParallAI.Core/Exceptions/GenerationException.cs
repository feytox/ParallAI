namespace ParallAI.Core.Exceptions;

public class GenerationException(string message, string apiMessage, string providerName, string modelName)
    : Exception(message)
{
    public string ApiMessage { get; } = apiMessage;
    public string ProviderName { get; } = providerName;
    public string ModelName { get; } = modelName;
}
