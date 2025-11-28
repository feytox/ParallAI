using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Commands;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> commands;
    public readonly IEnumerable<CommandAttribute> CommandsDescription;

    public CommandHandler(IEnumerable<ICommand> commands, IEnumerable<CommandAttribute> commandsDescription)
    {
        this.commands = commands
            .Select(cmd => (cmd, attr: cmd.GetType().GetCustomAttribute<CommandAttribute>()))
            .Where(t => t.attr is not null)
            .ToDictionary(t => t.attr!.Name, t => t.cmd, StringComparer.OrdinalIgnoreCase);
        
        CommandsDescription = commandsDescription;
    }
    
    public async Task HandleCommand(Message message, ITelegramBotClient bot)
    {
        var messageText = message.Text ?? message.Caption;
        
        if (messageText is null)
        {
            await bot.SendMessage(message.Chat, "В твоём запросе нет текста, я не могу его обработать");
            return;
        }
        
        var commandText = messageText.Split(' ')[0];
        if (commands.TryGetValue(commandText, out var command))
            await command.Execute(message, bot);
        else
            await bot.SendMessage(message.Chat, $"Я не знаю команды `{commandText}`");
    }
}