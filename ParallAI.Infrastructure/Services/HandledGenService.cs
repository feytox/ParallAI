using ParallAI.Core.Entities;
using ParallAI.Core.Services;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.Services;

public class HandledGenService<THandler, TProvider>(Func<TProvider, AiModel, THandler> handlerFactory)
    : IProviderGenService<TProvider>
    where THandler : IGenerationHandler
    where TProvider : AiProvider
{
    public async Task<AiResponse> Generate(TProvider provider, AiModel model, 
        AiMessage[] aiMessages, PromptSettings promptSettings, CancellationToken cancellationToken)
    {
        var handler = handlerFactory(provider, model);
        return await handler.Generate(aiMessages, promptSettings, cancellationToken);
    }
}