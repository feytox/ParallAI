using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings.Provider;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class RequestSettingsHandler() : StandardSettingsHandler<RequestSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "request-settings";
    
    protected override string GetPartsMessage(RequestSettingsState state)
    {
        var stateInfo = string.Join('\n', state.ToTextLines());
        return $"Текущие настройки:\n{stateInfo}";
    }

    public override Task FinalizeSettings(RequestSettingsState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        var config = new RequestConfig(state.Model!, state.Preset, RequestMode.Single);
        user.StateMachine.Pop(reactivate: false);
        
        var requestState = new RequestState(config);
        user.StateMachine.Push(requestState);
        return Task.CompletedTask;
    }

    protected override Task SaveSettingsToUser(RequestSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        return Task.CompletedTask;
    }
    
    private static void CreateParts(SettingsPartsBuilder<RequestSettingsState> builder)
    {
        builder
            .AddSelect(CallbackTag, "Пресет", "Выберите пресет:", user => user.UserPresets, preset => preset.Name,
                (state, preset) => state.Preset = preset)
            .AddSelect(CallbackTag, "Модель", "Выберите модель:", user => user.UserModels, model => model.DisplayName,
                (state, model) => state.Model = model);
    }
}