using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🆚 Режим сравнения", MenuOrder.Compare)]
[Command("/compare", "глубокое сравнение запроса")]
public class CompareCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.ComparisonDates
            .Select(time => time.ToUniversalTime().ToString("yyyy/MM/dd HH:mm z"))
            .Select((text, i) => CompareCallback.CreateButton(text, $"{i}"))
            .Append(CompareCallback.CreateButton("+", "+"))
            .Chunk(1);

        await bot.SendMessage(message.Chat, "Выберите предыдущий конфиг сравнения или создайте новый:",
            replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}