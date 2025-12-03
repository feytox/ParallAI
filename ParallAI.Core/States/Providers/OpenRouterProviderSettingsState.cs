using ParallAI.Core.Providers;

namespace ParallAI.Core.States.Providers;

public class OpenRouterProviderSettingsState: ProviderSettingsState
{
    public string? Token { get; set; }
}

public static class OpenRouterProviderSettingsExtensions
{
    public static OpenRouterProvider ToProvider(this OpenRouterProviderSettingsState state) => new(state.Token!);
    
    public static OpenRouterProviderSettingsState ToState(this OpenRouterProvider provider) => new()
    {
        Token = provider.Token
    };
}