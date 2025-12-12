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
[Command("/ask", "выбор режима запроса к модели")]
public class AskCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(ChatId chatId, ITelegramBotClient bot, User user)
    {
        var buttons = new InlineKeyboardMarkup([
            [AskCallback.Create("Одиночный запрос", "single")],
            [AskCallback.Create("Непрерывные запросы", "continuous")],
            [AskCallback.Create("Одиночный запрос с настройкой параметров", "settings")],
        ]);
        
        await bot.SendMessage(chatId, "Выберите режим запросов", replyMarkup: buttons);
    }
}