namespace AICore.ValueTypes;

public record OpenAICompatibleProvider(Uri EndpointUrl, string Token) : AiProvider;