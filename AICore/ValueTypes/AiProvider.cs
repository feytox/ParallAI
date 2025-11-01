namespace AICore.ValueTypes;

public abstract class AiProvider
{
    public abstract AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings);
}