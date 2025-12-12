using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(CompareSettingsHandler.CallbackTag)]
public class CompareSettingsStateCallback(IRepository<User, long> users, CompareSettingsHandler handler) 
    : SettingsStateCallback<CompareSettingsState, CompareSettingsHandler>(users, handler)
{
    protected override async Task<bool> HandleDataContent(CompareSettingsState state, CallbackQuery query,
        string content,
        ITelegramBotClient bot, User user)
    {
        if (content == "s")
        {
            await Handler.FinalizeSettings(state, query, bot, user);
            return true;
        }

        if (!content.StartsWith('-'))
            return false;

        var elementIndex = -int.Parse(content) - 1;
        Handler.RemoveCompareElement(state, elementIndex);
        return true;
    }
}