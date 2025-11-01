#region

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public interface ICommand
{
    public Task Execute(Message message, ITelegramBotClient bot);
}