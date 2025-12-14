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
    public const string CancelPartTag = "cancelPart_provider_openai";

    private static void CreateParts(SettingsPartsBuilder<OpenAICompatibleSettingsState> builder)
    {
        builder.AddSimple("API ключ", CancelPartTag,"Введите API ключ провайдера", "",
            text => text, state => state.Token);
        builder.AddSimple("Endpoint Url", CancelPartTag,"Введите Endpoint URL", "Это не похоже на URL. Попробуйте снова", 
            text => Uri.TryCreate(text, UriKind.Absolute, out var uri) ? uri : null, 
            state => state.Endpoint);
    }

    protected override Task<bool> SaveSettingsToUser(OpenAICompatibleSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(true);
    }

    protected override string GetPartsMessage(OpenAICompatibleSettingsState state)
    {
        return "Настройки провайдера:\n\n" + state.ToFormattedString();
    }
}