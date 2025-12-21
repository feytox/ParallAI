using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Commands;

[Command("/start", "выводит главное меню")]
[Command("/help", "выводит главное меню")]
public class MainMenuCommand(Lazy<MainMenuCommandsStorage> commandsStorage) : ICommand
{
    public async Task Execute(ChatId chatId, long userId, ITelegramBotClient bot)
    {
        await SendMenu(chatId, bot, commandsStorage.Value.Keyboard);
    }

    public static async Task SendMenu(ChatId chatId, ITelegramBotClient bot, ReplyKeyboardMarkup keyboardMarkup)
    {
        await bot.SendMessage(
            chatId: chatId,
            text: "\u3164",
            replyMarkup: keyboardMarkup
        );
    }
}