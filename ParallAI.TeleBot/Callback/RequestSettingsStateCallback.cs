using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(RequestSettingsHandler.CallbackTag)]
public class RequestSettingsStateCallback(IRepository<User, long> users, RequestSettingsHandler handler)
    : SettingsStateCallback<RequestSettingsState, RequestSettingsHandler>(users, handler)
{
    protected override async Task<bool> HandleDataContent(RequestSettingsState state, CallbackQuery query,
        string content,
        ITelegramBotClient bot, User user)
    {
        if (content != "c") 
            return content.StartsWith('-');
        
        await Handler.FinalizeSettings(state, query, bot, user);
        return true;
    }
}
    