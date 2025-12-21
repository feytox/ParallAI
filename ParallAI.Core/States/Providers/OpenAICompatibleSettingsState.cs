using ParallAI.Core.Providers;

namespace ParallAI.Core.States.Providers;

public class OpenAICompatibleSettingsState : ProviderSettingsState
{
    public string? Token { get; set; }

    public Uri? Endpoint { get; set; }
}

public static class OpenAIProviderSettingsExtensions
{
    public static OpenAICompatibleProvider ToProvider(this OpenAICompatibleSettingsState state)
        => new(state.Endpoint!, state.Token!);

    public static OpenAICompatibleSettingsState ToState(this OpenAICompatibleProvider provider) => new()
    {
        Token = provider.Token,
        Endpoint = provider.EndpointUrl
    };
}