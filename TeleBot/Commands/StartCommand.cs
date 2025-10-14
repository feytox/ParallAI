using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

public class StartCommand : ICommand
{
    public string Name => "/start";

    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Привет! Я ParallAI!");
    }
}