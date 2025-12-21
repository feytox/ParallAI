using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Services;

public interface IGenService
{
    public Type ProviderType { get; }

    public Task<AiResponse> Generate(AiProvider provider, AiModel model, AiMessage[] aiMessage,
        PromptSettings promptSettings, CancellationToken cancellationToken);
}