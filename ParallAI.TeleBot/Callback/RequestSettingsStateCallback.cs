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
    protected override async Task<bool> HandleDataContent(RequestSettingsState state, ChatId chatId, string content, ITelegramBotClient bot, User user)
    {
        if (content == "c")
        {
            await Handler.FinalizeSettings(state, chatId, bot, user);
            return true;
        }

        if (!content.StartsWith('-'))
            return false;
        
        return true;
    }
}
    