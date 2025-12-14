using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class GeminiProviderSettingsHandler()
    : StandardSettingsHandler<GeminiProviderSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "gemini_provider_settings";
    public const string CancelPartTag = "cancelPart_provider_gemini";
    

    private static void CreateParts(SettingsPartsBuilder<GeminiProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", CancelPartTag,"Введите API ключ провайдера", "",
            text => text, state => state.Token);
    }

    protected override Task<bool> SaveSettingsToUser(GeminiProviderSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(true);
    }

    protected override string GetPartsMessage(GeminiProviderSettingsState state)
    {
        return "Настройки провайдера:\n\n" + state.ToFormattedString();
    }
}