using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class MainMenuStateAction : StateAction<MainMenuState>
{
    protected override async Task<bool> Execute(MainMenuState state, Message message, ITelegramBotClient bot, User user)
    {
        if (message.Text == "Модели")
            await bot.SendMessage(
                chatId: message.Chat,
                text: "sosi"
            );
        return false;
    }

    protected override async Task<bool> ExecuteAfter(MainMenuState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        if (!state.Reactivated)
            return false;
        
        var keyboard = new ReplyKeyboardMarkup([
            [new KeyboardButton("Модели"), new KeyboardButton("Пресеты")],
            [new KeyboardButton("Конфигурация"), new KeyboardButton("Запрос")],
            [new KeyboardButton("Сравнение")]
        ])
        {
            ResizeKeyboard = true
        };

        await bot.SendMessage(chatId, "Привет, я параллаич! (плейсхолдер)");
        await bot.SendMessage(
            chatId: chatId,
            text: "Главное меню:",
            replyMarkup: keyboard
        );

        state.Reactivated = false;
        return true;
    }
}