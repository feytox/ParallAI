using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

public record OpenAiResponse(OpenAiResponse.Choice[] Choices) : IGenResponse
{
    public record Choice(OpenAiMessage Message);

    public AiResponse ToTextResponse()
    {
        var choice = Choices.SingleOrDefault();
        if (choice is null)
            throw new ArgumentException(
                $"Response should contain exactly 1 response choice. Actual: {Choices.Length}");
        
        return new AiResponse(choice.Message.Content);
    }
}