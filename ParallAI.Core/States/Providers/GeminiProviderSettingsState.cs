using ParallAI.Core.Providers;

namespace ParallAI.Core.States;

public class GeminiProviderSettingsState : ProviderSettingsState
{
    public string? Token { get; set; }
}

public static class GeminiProviderSettingsExt
{
    public static GeminiProvider ToProvider(this GeminiProviderSettingsState state) => new(state.Token!);
    
    public static GeminiProviderSettingsState ToState(this GeminiProvider provider) => new()
    {
        Token = provider.Token
    };
}