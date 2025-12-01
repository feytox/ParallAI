using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class OpenRouterSettingsHandler() : SettingsHandler<OpenRouterProviderSettingsState>(Tag, CreateParts)
{
    public const string Tag = "openrouter_provider_settings";

    private static void CreateParts(SettingsPartsBuilder<OpenRouterProviderSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "",
            text => text, (state, value) => state.Token = value);
    }

    protected override Task SaveSettingsToUser(OpenRouterProviderSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user) => Task.CompletedTask;

    protected override string GetPartsMessage(OpenRouterProviderSettingsState state) =>
        "Настройки провайдера"; // TODO: добавить отображение текущих настроек
}