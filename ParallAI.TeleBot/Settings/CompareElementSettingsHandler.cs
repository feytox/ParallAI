using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class CompareElementSettingsHandler() 
    : StandardSettingsHandler<CompareElementSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "compare-element";
    
    protected override string GetPartsMessage(CompareElementSettingsState state)
    {
        var stateInfo = string.Join('\n', state.ToTextLines());
        return $"Текущие настройки:\n{stateInfo}";
    }

    protected override Task SaveSettingsToUser(CompareElementSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        return Task.CompletedTask;
    }
    
    private static void CreateParts(SettingsPartsBuilder<CompareElementSettingsState> builder)
    {
        builder
            .AddSelect(CallbackTag, "Пресет", "Выберите пресет:", user => user.UserPresets, preset => preset.Name,
                state => state.Preset)
            .AddSelect(CallbackTag, "Модель", "Выберите модель:", user => user.UserModels, model => model.DisplayName,
                state => state.Model);
    }
}