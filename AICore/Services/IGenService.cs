using AICore.Entities;
using AICore.ValueTypes;

namespace AICore.Services;

public interface IGenService
{
    public Type ProviderType { get; }
    public Task<AiResponse> Generate(AiProvider provider, AiModel model, Prompt prompt, PromptSettings promptSettings);
}