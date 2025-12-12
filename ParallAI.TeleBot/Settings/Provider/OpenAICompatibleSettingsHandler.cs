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
            text => text, (state, value) => state.Token = value);
        builder.AddSimple("EndpointUrl", "Введите EndPoint URL", "Это не похоже на URL. Попробуйте снова", 
            text => Uri.TryCreate(text, UriKind.Absolute, out var uri) ? uri : null,
            (state, uri) => state.Endpoint = uri);
    }

    protected override Task<bool> SaveSettingsToUser(OpenAICompatibleSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(true);
    }

    protected override string GetPartsMessage(OpenAICompatibleSettingsState state)
    {
        return "Настройки провайдера"; // TODO: добавить отображение текущих настроек
    }
}