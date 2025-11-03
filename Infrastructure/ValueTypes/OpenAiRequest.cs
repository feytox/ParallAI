using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

public record OpenAiRequest(string Model, OpenAiMessage[] Messages, double Temperature)
{
    public static OpenAiRequest Create(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        var messages = CreateMessages(prompt, promptSettings).ToArray();
        return new OpenAiRequest(modelId, messages, promptSettings.Temperature);
    }

    private static IEnumerable<OpenAiMessage> CreateMessages(Prompt prompt, PromptSettings promptSettings)
    {
        if (promptSettings.HasSystemInstruction)
            yield return new OpenAiMessage(promptSettings.SystemInstructions, "system");

        yield return new OpenAiMessage(prompt.Text, "user");
    }
}