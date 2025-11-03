using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

public interface IGenResponse
{
    AiResponse ToTextResponse();
}