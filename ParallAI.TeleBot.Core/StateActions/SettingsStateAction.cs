using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public class SettingsStateAction<TState>(SettingsHandler<TState> handler) : StateAction<TState>
    where TState : SettingsState
{
    protected override async Task<ActionResult> Execute(TState state, Message message, ITelegramBotClient bot, User user)
    {
        await handler.HandleMessage(state, message, bot, user);
        return ActionResult.Handled;
    }

    protected override Task<ActionResult> ExecuteAfter(TState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot, User user)
    {
        return handler.ExecuteAfter(state, chatId, prevMessage, bot);
    }
}