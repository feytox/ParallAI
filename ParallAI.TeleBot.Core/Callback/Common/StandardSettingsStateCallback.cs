using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class StandardSettingsStateCallback<TState, THandler>(IRepository<User, long> users, THandler handler) 
    : SettingsStateCallback<TState, THandler>(users, handler) 
    where TState : SettingsState 
    where THandler : StandardSettingsHandler<TState>
{
    protected override async Task<bool> HandleDataContent(TState state, ChatId chatId, string content, 
        ITelegramBotClient bot, User user)
    {
        if (content != "c")
            return false;
        
        await Handler.FinalizeSettings(state, chatId, bot, user);
        return true;
    }
}