using System.Reflection;
using TeleBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Services;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> commandsDict;
    private readonly IUserSessionService sessionService;

    public CommandHandler(IEnumerable<ICommand> commands, IUserSessionService sessionService)
    {
        this.sessionService = sessionService;
        commandsDict = commands
            .Select(cmd => (cmd, attr: cmd.GetType().GetCustomAttribute<CommandAttribute>()))
            .Where(t => t.attr is not null)
            .ToDictionary(t => t.attr!.Name, t => t.cmd, StringComparer.OrdinalIgnoreCase);
    }

    public async Task HandleCommand(Message message, ITelegramBotClient bot)
    {
        var chatId = message.Chat.Id;
        var messageText = message.Text ?? message.Caption;

        if (string.IsNullOrWhiteSpace(messageText)) return;
        
        var commandName = await sessionService.GetCommandToExecuteName(chatId, messageText);

        if (!commandsDict.TryGetValue(commandName, out var commandToExecute))
        {
            await bot.SendMessage(chatId, "Я не знаю такой команды. Введите /help для списка команд.");
            return;
        }

        var nextCommandInfo = await commandToExecute.Execute(message, bot);

        await sessionService.UpdateSession(chatId, nextCommandInfo);
    }
}