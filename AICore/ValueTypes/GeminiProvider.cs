namespace AICore.ValueTypes;

public record GeminiProvider(string Token) : AiProvider
{
    public override AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }
}