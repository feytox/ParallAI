using ParallAI.Core.States;
using ParallAI.Core.States.Common;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Commands;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class ProviderSettingsPart(string name)
    : SettingsPart<ModelSettingsState>(name), ICanSavePart<ModelSettingsState, ProviderSettingsState>, IValidatablePart<ModelSettingsState>
{
    public const string GeminiTag = "choose_gemini";
    public const string OpenAiTag = "choose_openai";
    public const string OpenRouterTag = "choose_openrouter";

    public override async Task<UserState?> ActivatePart(ModelSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        var buttons = new[]
            {
                InlineKeyboardButton.WithCallbackData("Gemini", GeminiTag),
                InlineKeyboardButton.WithCallbackData("OpenRouter", OpenRouterTag),
                InlineKeyboardButton.WithCallbackData("OpenAI совместимое", OpenAiTag)
            }
            .Chunk(2);

        await bot.EditCallbackMessage(query,
            $"Выберите провайдер\n\nГайд на получение API-ключей 👉 {ProviderGuideCommand.GuideHtmlUrl}",
            parseMode: ParseMode.Html,
            replyMarkup: new InlineKeyboardMarkup(buttons));
        return null;
    }
    
    public void SaveToState(ModelSettingsState state, ProviderSettingsState prevState)
    {
        state.Provider = prevState switch
        {
            GeminiProviderSettingsState geminiState => geminiState.ToProvider(),
            OpenAICompatibleSettingsState openAiState => openAiState.ToProvider(),
            OpenRouterProviderSettingsState openRouterState => openRouterState.ToProvider(),
            _ => throw new InvalidOperationException("Unknown previous state type")
        };
    }

    public bool Validate(ModelSettingsState state)
    {
        return state.Provider != null;
    }
}