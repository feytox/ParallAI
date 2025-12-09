using ParallAI.Core.Exceptions;
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
        builder.AddSimple("EndpointUrl", "Введите EndPoint URL", "", 
            text => text, SaveEndpoint);
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
    
    private static void SaveEndpoint(OpenAICompatibleSettingsState state, string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            throw new UserFriendlyException("Invalid endpoint URL", "Это не похоже на URL. Попробуйте снова");
        state.Endpoint = uri;
    }
}