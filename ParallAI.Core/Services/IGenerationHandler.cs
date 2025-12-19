using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Services;

public interface IGenerationHandler
{
    Task<AiResponse> Generate(AiMessage[] aiMessages, PromptSettings promptSettings);
}