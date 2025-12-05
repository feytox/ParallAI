using ParallAI.Core.States;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class CreateElementSettingsPart(string name) 
    : SettingsPart<CompareSettingsState>(name), ICanSavePart<CompareSettingsState, CompareElementSettingsState>
{
    public override Task<UserState?> ActivatePart(CompareSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        return Task.FromResult<UserState?>(new CompareElementSettingsState());
    }
    
    // TODO: add validation
    public void SaveToState(CompareSettingsState state, CompareElementSettingsState prevState)
    {
        var element = prevState.ToElement();
        state.AddElement(element);
    }
}