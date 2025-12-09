using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class OpenAICompatibleSettingsHandler() 
    : StandardSettingsHandler<OpenAICompatibleSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "openai_provider_settings";

    private static void CreateParts(SettingsPartsBuilder<OpenAICompatibleSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "",
            text => text, state => state.Token);
        builder.AddSimple("EndpointUrl", "Введите EndPoint URL", "", 
            text => text, state => state.Endpoint);
    }

    protected override Task SaveSettingsToUser(OpenAICompatibleSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        return Task.CompletedTask;
    }

    protected override string GetPartsMessage(OpenAICompatibleSettingsState state)
    {
        return "Настройки провайдера"; // TODO: добавить отображение текущих настроек
    }
}