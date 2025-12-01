using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(ProviderSettingsPart.OpenAiTag)]
public class OpenAiChooseCallback(IRepository<User, long> users, SettingsHandler<OpenAICompatibleSettingsState> handler) :
    ProviderChooseCallback<OpenAICompatibleSettingsState>(users, handler)
{
    protected override OpenAICompatibleSettingsState ToSettingsState(AiProvider provider)
    {
        if (provider is OpenAICompatibleProvider openAiCompatibleProvider)
            return openAiCompatibleProvider.ToState();
        return new OpenAICompatibleSettingsState();
    }
}