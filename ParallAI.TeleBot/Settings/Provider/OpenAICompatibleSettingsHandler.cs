using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Util;
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
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "Ошибка: ключ должен содержать только ASCII",
            text => text.ParseAscii(), state => state.Token);
        builder.AddSimple("Endpoint Url", "Введите Endpoint URL", "Это не похоже на URL. Попробуйте снова",
            text => Uri.TryCreate(text, UriKind.Absolute, out var uri) ? uri : null,
            state => state.Endpoint);
    }

    protected override Task<ActionResult> SaveSettingsToUser(OpenAICompatibleSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(ActionResult.Handled);
    }

    protected override string GetPartsMessage(OpenAICompatibleSettingsState state)
    {
        return "Настройки провайдера:\n\n" + state.ToFormattedString();
    }
}