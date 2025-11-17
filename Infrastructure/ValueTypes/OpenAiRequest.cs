using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://platform.openai.com/docs/api-reference/chat/create">OpenAI API Reference</see>
/// </remarks>
public record OpenAiRequest(string Model, OpenAiMessage[] Messages, double Temperature)
{
    public static OpenAiRequest Create(string modelId, TextPrompt prompt, PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(prompt, promptSettings).ToArray();
        return new OpenAiRequest(modelId, messages, promptSettings.Temperature);
    }

    public static OpenAiRequest Create(string modelId, FilePrompt prompt, IEnumerable<AiFile> files,
        PromptSettings promptSettings)
    {
        var messages = OpenAiMessage.Create(prompt, files, promptSettings).ToArray();
        return new OpenAiRequest(modelId, messages, promptSettings.Temperature);
    }
}