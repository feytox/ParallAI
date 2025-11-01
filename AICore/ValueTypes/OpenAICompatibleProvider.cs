namespace AICore.ValueTypes;

public record OpenAICompatibleProvider(Uri EndpointUrl, string Token) : AiProvider
{
    public override AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }
}