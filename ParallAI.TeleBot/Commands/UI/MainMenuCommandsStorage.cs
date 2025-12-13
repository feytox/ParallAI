using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Commands.UI;

public class MainMenuCommandsStorage
{
    public Dictionary<string, ICommand> Commands { get; }
    public ReplyKeyboardMarkup Keyboard { get; }

    public MainMenuCommandsStorage(IEnumerable<(ICommand command, MainMenuAttribute attribute)> commands)
    {
        var sortedCommands = commands
            .OrderBy(t => t.attribute.Weight)
            .ThenBy(t => t.attribute.NameUI)
            .ToList();

        Commands = sortedCommands
            .ToDictionary(t => t.attribute.NameUI, t => t.command, StringComparer.OrdinalIgnoreCase);

        Keyboard = CreateKeyboard(sortedCommands.Select(t => t.attribute.NameUI));
    }

    private static ReplyKeyboardMarkup CreateKeyboard(IEnumerable<string> names)
    {
        var buttons = names
            .Select(name => new KeyboardButton(name))
            .Chunk(2);

        return new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
    }
}