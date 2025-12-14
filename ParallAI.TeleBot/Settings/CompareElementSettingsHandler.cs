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
    public const string CancelPartTag = "cancelPart_compareElement";

    protected override string GetPartsMessage(CompareElementSettingsState state)
    {
        var stateInfo = string.Join('\n', state.ToTextLines());
        return $"Текущие настройки:\n{stateInfo}";
    }

    protected override Task<bool> SaveSettingsToUser(CompareElementSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult(true);
    }

    private static void CreateParts(SettingsPartsBuilder<CompareElementSettingsState> builder)
    {
        builder
            .AddSelect(CallbackTag, CancelPartTag,"Пресет", "Выберите пресет:", 
                user => user.UserPresets, preset => preset.Name, state => state.Preset)
            .AddSelect(CallbackTag, CancelPartTag,"Модель", "Выберите модель:", 
                user => user.UserModels, model => model.DisplayName, state => state.Model);
    }
}