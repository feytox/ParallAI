using TeleBot.Commands;
using TeleBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot;

public interface ICommand
{
    Task<NextCommandInfo?> Execute(Message message, ITelegramBotClient bot);
}

public class NextCommandInfo
{
    public string? NextCommandName { get; set; }
}