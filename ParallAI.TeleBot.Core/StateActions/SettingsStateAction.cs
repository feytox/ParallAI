using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class SettingsStateAction<TState>(SettingsHandler<TState> handler) : StateAction<TState> 
    where TState : SettingsState
{
    protected override async Task<bool> Execute(TState state, Message message, ITelegramBotClient bot, User user)
    {
        await handler.HandleMessage(state, message, bot, user);
        return true;
    }

    protected override Task<bool> ExecuteAfter(TState state, ChatId chatId, Message? prevMessage, 
        ITelegramBotClient bot, User user)
    {
        return handler.ExecuteAfter(state, chatId, prevMessage, bot);
    }
}