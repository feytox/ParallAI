using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/object">OpenAI API Reference</see>
/// </remarks>
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