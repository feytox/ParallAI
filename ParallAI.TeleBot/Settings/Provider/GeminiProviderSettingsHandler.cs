using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class GeminiProviderSettingsHandler()
    : StandardSettingsHandler<GeminiProviderSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "gemini_provider_settings";

    private static void CreateParts(SettingsPartsBuilder<GeminiProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "Ошибка: ключ должен содержать только ASCII",
            text => text.ParseAscii(), state => state.Token);
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