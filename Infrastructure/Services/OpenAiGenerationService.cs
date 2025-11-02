using AICore.Services;
using AICore.ValueTypes;

namespace Infrastructure.Services;

public class OpenAiGenerationService : IAiGenerationService<OpenAICompatibleProvider>
{
    public async Task<AiResponse> Generate(OpenAICompatibleProvider provider, string modelId, 
        Prompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }
}