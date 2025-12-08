using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Commands;

[MainMenu("💬 Запрос к модели", MenuOrder.Ask)]
[Command("/requestmode", "выбор режима запроса к модели")]
public class RequestModeCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = new InlineKeyboardMarkup([
            [InlineKeyboardButton.WithCallbackData(
                "Одиночный запрос", $"{AskCallback.CallbackTag}:single")],
            [InlineKeyboardButton.WithCallbackData(
                "Непрерывные запросы", $"{AskCallback.CallbackTag}:continuous")],
            [InlineKeyboardButton.WithCallbackData(
                "Одиночный запрос с настройкой параметров", $"{AskCallback.CallbackTag}:settings")]
        ]);
        await bot.SendMessage(message.Chat, "Выберите режим запросов",
            replyMarkup: buttons);
    }
}