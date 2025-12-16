using ParallAI.Core.States;
using ParallAI.TeleBot.Commands;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class MainMenuStateAction(MainMenuCommandsStorage commandsStorage) : StateAction<MainMenuState>
{
    protected override async Task<bool> Execute(MainMenuState state, Message message, ITelegramBotClient bot, User user)
    {
        var messageText = message.Text ?? message.Caption;
        if (messageText == null || !commandsStorage.Commands.TryGetValue(messageText, out var command)) 
            return false;
        
        await command.Execute(message.Chat, message.From!.Id, bot);
        return true;
    }

    protected override async Task<bool> ExecuteAfter(MainMenuState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot, User user)
    {
        if (!state.Reactivated)
            return false;

        await MainMenuCommand.SendMenu(chatId, bot, commandsStorage.Keyboard);

        state.Reactivated = false;
        return true;
    }
}