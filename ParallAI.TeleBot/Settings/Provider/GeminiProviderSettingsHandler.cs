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

    private static void CreateParts(SettingsPartsBuilder<GeminiProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "",
            text => text, state => state.Token);
    }

    protected override Task SaveSettingsToUser(GeminiProviderSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        return Task.CompletedTask;
    }

    protected override string GetPartsMessage(GeminiProviderSettingsState state)
    {
        return "Настройки провайдера"; // TODO: добавить отображение текущих настроек
    }
}