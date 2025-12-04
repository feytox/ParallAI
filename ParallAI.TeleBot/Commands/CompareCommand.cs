using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🆚 Режим сравнения", MenuOrder.Compare)]
//[Command("/compare", "глубокое сравнение запроса")]
public class CompareCommand : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Я ещё не реализован, братик :3");
    }
}