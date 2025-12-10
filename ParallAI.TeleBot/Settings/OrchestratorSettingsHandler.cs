using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class OrchestratorSettingsHandler() 
    : StandardSettingsHandler<OrchestratorSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "orchestrator-settings";
    private const int Limit = 3;
    
    protected override string GetPartsMessage(OrchestratorSettingsState state) => "Настройки оркестратора:";

    protected override Task<bool> SaveSettingsToUser(OrchestratorSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        var config = state.Build();
        user.AddComparison(config, Limit);
        user.StateMachine.Pop(reactivate: false);
        
        var nextState = new CompareState(config);
        user.StateMachine.Push(nextState);
        return Task.FromResult(false);
    }
    
    private static void CreateParts(SettingsPartsBuilder<OrchestratorSettingsState> builder)
    {
        builder
            .AddSelect(CallbackTag, "Пресет", "Выберите пресет:", user => user.UserPresets, preset => preset.Name,
                state => state.Preset)
            .AddSelect(CallbackTag, "Модель", "Выберите модель:", user => user.UserModels, model => model.DisplayName,
                state => state.Model);
    }
}