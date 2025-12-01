using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class OpenAICompatibleSettingsHandler() : SettingsHandler<OpenAICompatibleSettingsState>(Tag, CreateParts)
{
    public const string Tag = "openai_provider_settings";

    private static void CreateParts(SettingsPartsBuilder<OpenAICompatibleSettingsState> builder)
    {
        builder.AddSimple("API ключ", "Введите API ключ провайдера", "",
            text => text, (state, value) => state.Token = value);
        builder.AddSimple("EndpointUrl", "Введите EndPoint URL", "", 
            text => text, (state, value) => state.Endpoint = value);
    }

    protected override Task SaveSettingsToUser(OpenAICompatibleSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user) => Task.CompletedTask;

    protected override string GetPartsMessage(OpenAICompatibleSettingsState state) =>
        "Настройки провайдера"; // TODO: добавить отображение текущих настроек
}