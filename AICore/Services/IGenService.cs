using AICore.ValueTypes;

namespace AICore.Services;

public interface IGenService
{
    public Type ProviderType { get; }
    public Task<AiResponse> Generate(AiProvider provider, string modelId, Prompt prompt, PromptSettings promptSettings);
}