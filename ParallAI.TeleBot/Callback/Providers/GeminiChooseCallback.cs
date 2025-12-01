using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(ProviderSettingsPart.GeminiTag)]
public class GeminiChooseCallback(IRepository<User, long> users, SettingsHandler<GeminiProviderSettingsState> handler):
    ProviderChooseCallback<GeminiProviderSettingsState>(users, handler)
{
    protected override GeminiProviderSettingsState ToSettingsState(AiProvider provider)
    {
        if (provider is GeminiProvider geminiProvider)
            return geminiProvider.ToState();
        return new GeminiProviderSettingsState();
    }
}