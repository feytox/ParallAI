using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;

namespace ParallAI.TeleBot.Callback;

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