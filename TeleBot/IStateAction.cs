#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public interface IStateAction
{
    UserStateType HandledState { get; }
    Task Execute(Message message, User user, ITelegramBotClient bot);
}