using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(ProviderSettingsPart.OpenRouterTag)]
public class OpenRouterChooseCallBack(
    IRepository<User, long> users,
    SettingsHandler<OpenRouterProviderSettingsState> handler) :
    ProviderChooseCallback<OpenRouterProviderSettingsState>(users, handler)
{
    protected override OpenRouterProviderSettingsState ToSettingsState(AiProvider provider)
    {
        if (provider is OpenRouterProvider openRouterProvider)
            return openRouterProvider.ToState();
        return new OpenRouterProviderSettingsState();
    }
}