using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Commands;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> commands;
    private readonly HashSet<string> highPriorityCommands;

    public CommandHandler(IEnumerable<(ICommand command, CommandAttribute attribute)> commands)
    {
        var commandsAttributes = commands.ToArray();
        
        this.commands = commandsAttributes
            .ToDictionary(t => t.attribute.Name, t => t.command, StringComparer.OrdinalIgnoreCase);

        highPriorityCommands = commandsAttributes
            .Where(t => t.attr!.HighPriority)
            .Select(t => t.attr!.Name)
            .ToHashSet();
    }

    public async Task HandleCommand(Message message, ITelegramBotClient bot)
    {
        var commandText = GetCommandText(message);

        if (commandText is null)
        {
            await bot.SendMessage(message.Chat, "В твоём запросе нет текста, я не могу его обработать");
            return;
        }

        
        if (commands.TryGetValue(commandText, out var command))
            await command.Execute(message, bot);
        else
            await bot.SendMessage(message.Chat, $"Я не знаю команды `{commandText}`");
    }

    public bool IsHighPriorityCommand(Message message)
    {
        var commandText = GetCommandText(message);
        return commandText is not null && highPriorityCommands.Contains(commandText);
    }

    private static string? GetCommandText(Message message)
    {
        var messageText = message.Text ?? message.Caption;
        return messageText?.Split(' ')[0];
    }
}