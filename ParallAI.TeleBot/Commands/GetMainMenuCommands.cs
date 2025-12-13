using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Commands;

public abstract class GetMainMenuCommand(Lazy<MainMenuCommandsStorage> commandsStorage) : ICommand
{
    public async Task Execute(ChatId chatId, long userId, ITelegramBotClient bot)
    {
        await bot.SendMessage(
            chatId: chatId,
            text: "\u3164", // TODO: тут мб будет красивое первое сообщение (Дима Комаров обязательно его придумает)
            replyMarkup: commandsStorage.Value.Keyboard
        );
    }
}

[Command("/start", "выводит главное меню")]
public class StartCommand(Lazy<MainMenuCommandsStorage> commandsStorage) : GetMainMenuCommand(commandsStorage);

[Command("/help", "выводит главное меню")]
public class HelpCommand(Lazy<MainMenuCommandsStorage> commandsStorage) : GetMainMenuCommand(commandsStorage);