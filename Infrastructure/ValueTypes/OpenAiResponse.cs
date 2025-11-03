using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

public record OpenAiResponse(OpenAiResponse.Choice[] Choices)
{
    public record Choice(OpenAiMessage Message);
}

public static class OpenAiResponseExt
{
    public static AiResponse ToTextResponse(this OpenAiResponse response)
    {
        var choice = response.Choices.SingleOrDefault();
        if (choice is null)
            throw new ArgumentException(
                $"Response should contain exactly 1 response choice. Actual: {response.Choices.Length}");
        
        return new AiResponse(choice.Message.Content);
    }
}