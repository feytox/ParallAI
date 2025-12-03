using ParallAI.Core.Providers;

namespace ParallAI.Core.States.Providers;

public class OpenAICompatibleSettingsState : ProviderSettingsState
{
    public string? Token { get; set; }
    
    public string? Endpoint { get; set; }
}

public static class OpenAIProviderSettingsExtensions
{
    public static OpenAICompatibleProvider ToProvider(this OpenAICompatibleSettingsState state) => new(new Uri(state.Endpoint!), state.Token!);
    
    public static OpenAICompatibleSettingsState ToState(this OpenAICompatibleProvider provider) => new()
    {
        Token = provider.Token,
        Endpoint = provider.EndpointUrl.AbsoluteUri
    };
}