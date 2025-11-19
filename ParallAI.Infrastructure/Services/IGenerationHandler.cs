using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public interface IGenerationHandler
{
    Task<AiResponse> Generate(Prompt prompt, PromptSettings promptSettings);
}