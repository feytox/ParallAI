using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Providers;

public record OpenAICompatibleProvider(Uri EndpointUrl, string Token) : AiProvider;