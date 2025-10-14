using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

public class HelpCommand : ICommand
{
    public string Name => "/help";
    
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Мои команды:\n/start\n/help\n");
    }
}