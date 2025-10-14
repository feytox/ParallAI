using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot;

public interface ICommand
{
    public string Name { get; }
    public Task Execute(Message message, ITelegramBotClient bot);
}