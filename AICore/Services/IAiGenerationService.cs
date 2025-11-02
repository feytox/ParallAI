using AICore.ValueTypes;

namespace AICore.Services;

public interface IAiGenerationService<in TProvider> where TProvider : AiProvider
{
    public Task<AiResponse> Generate(TProvider provider, string modelId, Prompt prompt, PromptSettings promptSettings);
}