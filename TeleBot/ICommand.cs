#region

using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public interface ICommand
{
    Task Execute(Message message, ITelegramBotClient bot, User user);
}