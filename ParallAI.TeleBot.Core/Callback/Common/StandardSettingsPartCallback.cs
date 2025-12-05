using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class StandardSettingsPartCallback<TState, THandler>(IRepository<User, long> users, THandler handler) 
    : SettingsPartCallback<TState, THandler>(users, handler) 
    where TState : SettingsState 
    where THandler : StandardSettingsHandler<TState>
{
    protected override async Task<bool> HandleDataContent(TState state, ChatId chatId, string content, 
        ITelegramBotClient bot, User user)
    {
        if (content != "c")
            return false;
        
        await Handler.SaveSettings(state, chatId, bot, user);
        return true;
    }
}