using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Commands.Common;

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
            .Where(t => t.attribute.HighPriority)
            .Select(t => t.attribute.Name)
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

        await HandleCommand(commandText, message.Chat, message.From!.Id, bot);
    }

    public async Task HandleCommand(string commandText, ChatId chatId, long userId, ITelegramBotClient bot)
    {
        if (commands.TryGetValue(commandText, out var command))
            await command.Execute(chatId, userId, bot);
        else
            await bot.SendMessage(chatId, $"Я не знаю команды `{commandText}`");
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