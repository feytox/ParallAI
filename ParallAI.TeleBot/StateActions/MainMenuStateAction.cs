using ParallAI.Core.States;
using ParallAI.TeleBot.Commands.UI;
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

    public MainMenuStateAction(IEnumerable<(ICommand command, MainMenuAttribute attribute)> commands)
    {
        var sortedCommands = commands
            .OrderBy(t => t.attribute.Weight)
            .ThenBy(t => t.attribute.NameUI)
            .ToList();

        this.commands = sortedCommands
            .ToDictionary(t => t.attribute.NameUI, t => t.command, StringComparer.OrdinalIgnoreCase);

        keyboard = CreateKeyboard(sortedCommands.Select(t => t.attribute.NameUI));
    }

    protected override async Task<ActionResult> Execute(MainMenuState state, Message message, 
        ITelegramBotClient bot, User user)
    {
        var messageText = message.Text ?? message.Caption;
        if (messageText == null || !commands.TryGetValue(messageText, out var command)) 
            return ActionResult.Skipped;
        
        await command.Execute(message, bot);
        return ActionResult.Handled;
    }

    protected override async Task<ActionResult> ExecuteAfter(MainMenuState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        if (!state.Reactivated)
            return ActionResult.Skipped;
        
        await bot.SendMessage(
            chatId: chatId,
            text: "\u3164", // TODO: тут мб будет красивое первое сообщение (Дима Комаров обязательно его придумает)
            replyMarkup: keyboard
        );

        state.Reactivated = false;
        return ActionResult.Handled;
    }

    private static ReplyKeyboardMarkup CreateKeyboard(IEnumerable<string> names)
    {
        var buttons = names
            .Select(name => new KeyboardButton(name))
            .Chunk(2);

        return new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
    }
}