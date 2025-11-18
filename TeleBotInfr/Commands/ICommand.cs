using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBotInfr.Commands;

public interface ICommand
{
    Task Execute(Message message, ITelegramBotClient bot);
}