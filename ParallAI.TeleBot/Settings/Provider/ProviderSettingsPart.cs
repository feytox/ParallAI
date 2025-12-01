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
    public const string GeminiTag = "choose_gemini";
    public const string OpenAiTag = "choose_openai";
    public const string OpenRouterTag = "choose_openrouter";
    public override async Task<UserState?> ActivatePart(ModelSettingsState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        InlineKeyboardButton[] buttons = [
            InlineKeyboardButton.WithCallbackData("GeminiProvider", GeminiTag),
            InlineKeyboardButton.WithCallbackData("OpenAiCompatibleProvider", OpenAiTag),
            InlineKeyboardButton.WithCallbackData("OpenRouterProvider", OpenRouterTag)];

        await bot.SendMessage(chatId, "Выберите провайдер", replyMarkup: new InlineKeyboardMarkup(buttons));
        return null;
    }

    public override void SaveToState(ModelSettingsState state, UserState prevState)
    {
        state.Provider = prevState switch
        {
            GeminiProviderSettingsState geminiState => geminiState.ToProvider(),
            OpenAICompatibleSettingsState openAiState => openAiState.ToProvider(),
            OpenRouterProviderSettingsState openRouterState => openRouterState.ToProvider(),
            _ => throw new InvalidOperationException("Unknown previous state type")
        };
    }
}