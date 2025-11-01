namespace AICore.ValueTypes;

public abstract record AiProvider
{
    public abstract AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings);
}