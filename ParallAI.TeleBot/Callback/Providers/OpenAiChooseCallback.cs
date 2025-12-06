using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(ProviderSettingsPart.OpenAiTag)]
public class OpenAiChooseCallback(IRepository<User, long> users) 
    : ProviderChooseCallback<OpenAICompatibleSettingsState>(users)
{
    protected override OpenAICompatibleSettingsState ToSettingsState(AiProvider provider)
    {
        if (provider is OpenAICompatibleProvider openAiCompatibleProvider)
            return openAiCompatibleProvider.ToState();
        return new OpenAICompatibleSettingsState();
    }
}