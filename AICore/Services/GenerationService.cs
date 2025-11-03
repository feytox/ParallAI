using AICore.Entities;
using AICore.ValueTypes;

namespace AICore.Services;

public class GenerationService(IEnumerable<IGenService> genServices)
{
    public async Task<AiResponse> Generate(AiModel model, Prompt prompt, PromptSettings promptSettings)
    {
        var providerType = model.Provider.GetType();
        var genService = genServices.FirstOrDefault(service => service.ProviderType.IsAssignableFrom(providerType));
        if (genService is null)
            throw new ArgumentException($"IGenService for {providerType} not found.");

        return await genService.Generate(model.Provider, model.ModelId, prompt, promptSettings);
    }
}