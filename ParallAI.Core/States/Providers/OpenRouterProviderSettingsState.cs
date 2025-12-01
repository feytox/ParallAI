using ParallAI.Core.Providers;

namespace ParallAI.Core.States;

public class OpenRouterProviderSettingsState: ProviderSettingsState
{
    public string? Token { get; set; }
}

public static class OpenRouterProviderSettingsExt
{
    public static OpenRouterProvider ToProvider(this OpenRouterProviderSettingsState state) => new(state.Token!);
    
    public static OpenRouterProviderSettingsState ToState(this OpenRouterProvider provider) => new()
    {
        Token = provider.Token
    };
}