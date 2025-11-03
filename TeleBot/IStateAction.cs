#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public interface IStateAction
{
    bool CanHandle(UserState? state);

    Task Execute(Message message, ITelegramBotClient bot, User user);
}