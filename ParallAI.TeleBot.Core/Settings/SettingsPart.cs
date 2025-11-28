using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class SettingsPart<TState>(string name) where TState : SettingsState
{
    public string Name { get; } = name;
    
    public abstract Task<UserState?> ActivatePart(TState state, ChatId chatId, ITelegramBotClient bot, User user);
    public abstract void SaveToState(TState state, UserState prevState);
}