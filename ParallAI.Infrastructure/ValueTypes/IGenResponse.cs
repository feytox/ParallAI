using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

public interface IGenResponse
{
    AiResponse ToTextResponse();
}