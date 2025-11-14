using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands.Common;

public interface ICommand
{
    Task Execute(Message message, ITelegramBotClient bot);
}