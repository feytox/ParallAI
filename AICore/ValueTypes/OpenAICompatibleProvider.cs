namespace AICore.ValueTypes;

public class OpenAICompatibleProvider(Uri endpointUrl, string token) : AiProvider
{
    public Uri EndpointUrl { get; } = endpointUrl;
    public string Token { get; } = token;

    public override AiResponse Generate(string modelId, Prompt prompt, PromptSettings promptSettings)
    {
        throw new NotImplementedException();
    }
}