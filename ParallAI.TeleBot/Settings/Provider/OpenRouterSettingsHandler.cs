using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class OpenRouterSettingsHandler()
    : StandardSettingsHandler<OpenRouterProviderSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "openrouter_provider_settings";

    private static void CreateParts(SettingsPartsBuilder<OpenRouterProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "Ошибка: ключ должен содержать только ASCII",
            text => text.ParseAscii(), state => state.Token);
    }

    protected override Task<bool> SaveSettingsToUser(OpenRouterProviderSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(true);
    }

    protected override string GetPartsMessage(OpenRouterProviderSettingsState state)
    {
        return "Настройки провайдера:\n\n" + state.ToFormattedString();
    }
}