using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Services;

public interface IGenerationService
{
    public Task<AiResponse> Generate(
        AiModel model, AiMessage[] aiMessage, PromptSettings promptSettings, CancellationToken cancellationToken);
}