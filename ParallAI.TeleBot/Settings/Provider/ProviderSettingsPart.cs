using ParallAI.Core.States;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Commands;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings.Provider;

public class ProviderSettingsPart(string name, string cancelTag)
    : StandardSettingsPart<ModelSettingsState>(name, cancelTag, ParseMode.Html), 
        ICanSavePart<ModelSettingsState, ProviderSettingsState>, IValidatablePart<ModelSettingsState>
{
    public const string GeminiTag = "choose_gemini";
    public const string OpenAiTag = "choose_openai";
    public const string OpenRouterTag = "choose_openrouter";

    protected override string InputMessage =>
        $"Выберите провайдер\n\nГайд на получение API-ключей 👉 {ProviderGuideCommand.GuideHtmlUrl}";

    protected override IEnumerable<InlineKeyboardButton>? GetInputButtons(User user) =>
    [
        InlineKeyboardButton.WithCallbackData("Gemini", GeminiTag),
        InlineKeyboardButton.WithCallbackData("OpenRouter", OpenRouterTag),
        InlineKeyboardButton.WithCallbackData("OpenAI совместимое", OpenAiTag)
    ];
    
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