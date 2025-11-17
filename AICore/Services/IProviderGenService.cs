using AICore.Entities;
using AICore.ValueTypes;

namespace AICore.Services;

public interface IProviderGenService<in TProvider> : IGenService where TProvider : AiProvider
{
    public Task<AiResponse> Generate(TProvider provider, AiModel model, Prompt prompt, PromptSettings promptSettings);

    Type IGenService.ProviderType => typeof(TProvider);
    
    Task<AiResponse> IGenService.Generate(AiProvider provider, AiModel model, Prompt prompt, PromptSettings promptSettings)
    {
        return Generate((TProvider)provider, model, prompt, promptSettings);
    }
}