namespace ParallAI.Core.ValueTypes;

public record OpenAICompatibleProvider(Uri EndpointUrl, string Token) : AiProvider;