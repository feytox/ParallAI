using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public interface IGenerationHandler
{
    Task<AiResponse> Generate(AiMessage aiMessage, PromptSettings promptSettings);
}