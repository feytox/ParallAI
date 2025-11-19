using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

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

        var items = choice.Message.Content.Items;
        var content = items.SingleOrDefault();
        if (content is not TextContentItem textContent)
            throw new ArgumentException(
                $"Response should contain exactly 1 response text content. Actual: {items.Length}");
        
        return new AiResponse(textContent.Text);
    }
}