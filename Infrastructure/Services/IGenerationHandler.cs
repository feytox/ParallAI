using AICore.ValueTypes;

namespace Infrastructure.Services;

public interface IGenerationHandler
{
    Task<AiResponse> Generate(Prompt prompt, PromptSettings promptSettings);
}