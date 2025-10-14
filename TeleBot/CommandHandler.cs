using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot;

public class CommandHandler(IEnumerable<ICommand> commands)
{
    public async Task HandleCommand(Message message, ITelegramBotClient bot)
    {
        var messageText = message.Text ?? message.Caption;
        
        if (messageText is null)
        {
            await bot.SendMessage(message.Chat, "В твоём запросе нет текста, я не могу его обработать");
            return;
        }
        
        var commandText = messageText.Split(' ')[0];
        var command = commands.FirstOrDefault(cm => cm.Name == commandText);

        if (command is null)
        {
            await bot.SendMessage(message.Chat, "Я не знаю такой команды");
            return;
        }
        
        await command.Execute(message, bot);
    }
}