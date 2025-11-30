using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Commands;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly HashSet<string> _highPriorityCommands;

    public CommandHandler(IEnumerable<ICommand> commands)
    {
        var commandsAttributes = commands
            .Select(cmd => (cmd, attr: cmd.GetType().GetCustomAttribute<CommandAttribute>()))
            .Where(t => t.attr is not null)
            .ToArray();
        
        _commands = commandsAttributes
            .ToDictionary(t => t.attr!.Name, t => t.cmd, StringComparer.OrdinalIgnoreCase);

        _highPriorityCommands = commandsAttributes
            .Where(t => t.attr!.HighPriority)
            .Select(t => t.attr!.Name)
            .ToHashSet();
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
        if (_commands.TryGetValue(commandText, out var command))
            await command.Execute(message, bot);
        else
            await bot.SendMessage(message.Chat, $"Я не знаю команды `{commandText}`");
    }

    public bool IsHighPriorityCommand(Message message)
    {
        var messageText = message.Text ?? message.Caption;
        if (messageText is null) return false;
        
        var commandText = messageText.Split(' ')[0];
        return _highPriorityCommands.TryGetValue(commandText, out _);
    }
}