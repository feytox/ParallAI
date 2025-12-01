using ParallAI.Core.Providers;
using ParallAI.Core.States;
using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public class ProviderSettingsPart(string name) : SettingsPart<ModelSettingsState>(name)
{
    public const string GeminiTag = "choosing_gemini";
    public override async Task<UserState?> ActivatePart(ModelSettingsState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        InlineKeyboardButton[] buttons = [InlineKeyboardButton.WithCallbackData("GeminiProvider", GeminiTag)];

        await bot.SendMessage(chatId, "Выберите провайдер", replyMarkup: new InlineKeyboardMarkup(buttons));
        return null;
    }

    public override void SaveToState(ModelSettingsState state, UserState prevState)
    {
        if (prevState is GeminiProviderSettingsState geminiState)
            state.Provider = geminiState.ToProvider();
    }
}