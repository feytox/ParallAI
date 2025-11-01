namespace AICore.ValueTypes;

public class GeminiProvider(string token) : AiProvider
{
    public string Token { get; private set; } = token;

    public override AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }
}