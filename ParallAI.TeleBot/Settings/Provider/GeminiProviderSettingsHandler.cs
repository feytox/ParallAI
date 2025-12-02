using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class GeminiProviderSettingsHandler(): SettingsHandler<GeminiProviderSettingsState>(Tag, CreateParts)
{
    public const string Tag = "gemini_provider_settings";
    
    private static void CreateParts(SettingsPartsBuilder<GeminiProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "",
            text => text, (state, value) => state.Token = value);
    }

    protected override Task SaveSettingsToUser(GeminiProviderSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user) => Task.CompletedTask;

    protected override string GetPartsMessage(GeminiProviderSettingsState state) => "Настройки провайдера"; // TODO: добавить отображение текущих настроек
}

