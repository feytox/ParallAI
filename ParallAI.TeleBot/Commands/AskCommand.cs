using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Commands;

[MainMenu("💬 Запрос к модели", MenuOrder.Ask)]
//[Command("/ask", "запрос к модели с заданной конфигурацией")]
public class AskCommand : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Я ещё не реализован, братик :3");
    }
}