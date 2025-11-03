#region

using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot.StateActions.Common;

public interface IStateAction
{
    bool CanHandle(UserState state);

    Task Execute(UserState state, Message message, ITelegramBotClient bot, User user);
}