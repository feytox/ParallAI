using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public interface IStateAction
{
    bool CanHandle(UserState state);

    Task<ActionResult> Execute(UserState state, Message message, ITelegramBotClient bot, User user);

    Task<ActionResult> ExecuteAfter(UserState state, ChatId chatId, Message? prevMessage, ITelegramBotClient bot, User user);
}