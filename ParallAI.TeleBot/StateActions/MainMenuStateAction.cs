using System.Reflection;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class MainMenuStateAction : StateAction<MainMenuState>
{
    private readonly Dictionary<string, ICommand> commands;
    
    private readonly ReplyKeyboardMarkup keyboard;
    
    public MainMenuStateAction(IEnumerable<ICommand> commands)
    {
        var sortedCommands = commands
            .Select(cmd => (cmd, attr: cmd.GetType().GetCustomAttribute<MainMenuAttribute>()))
            .Where(t => t.attr is not null)
            .OrderBy(t => t.attr!.Weight)
            .ThenBy(t => t.attr!.NameUI)
            .ToList();
        
        this.commands = sortedCommands
            .ToDictionary(t => t.attr!.NameUI, t => t.cmd, StringComparer.OrdinalIgnoreCase);
        
        var sortedButtonNames = sortedCommands.Select(t => t.attr!.NameUI);
        keyboard = CreateKeyboardFromCommands(sortedButtonNames);
    }
    
    protected override async Task<bool> Execute(MainMenuState state, Message message, ITelegramBotClient bot, User user)
    {
        var messageText = message.Text ?? message.Caption;

        if (messageText != null && commands.TryGetValue(messageText, out var command))
        {
            await command.Execute(message, bot);
            return true;
        }
        
        return false;
    }

    protected override async Task<bool> ExecuteAfter(MainMenuState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        if (!state.Reactivated)
            return false;

        await bot.SendMessage(chatId, "Привет, я параллаич! (плейсхолдер)");
        await bot.SendMessage(
            chatId: chatId,
            text: "Главное меню:",
            replyMarkup: keyboard
        );

        state.Reactivated = false;
        return true;
    }
    
    private static ReplyKeyboardMarkup CreateKeyboardFromCommands(IEnumerable<string> commands)
    {
        var buttons = new List<KeyboardButton[]>();
        var currentRow = new List<KeyboardButton>();

        foreach (var command in commands)
        {
            currentRow.Add(new KeyboardButton(command));

            if (currentRow.Count != 2) continue;
            
            buttons.Add(currentRow.ToArray());
            currentRow = [];
        }
        
        if (currentRow.Count > 0)
        {
            buttons.Add(currentRow.ToArray());
        }

        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true,
            InputFieldPlaceholder = "Главное меню"
        };
    }
}