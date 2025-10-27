using TeleBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

[Command("/start", "стартовая команда")]
public class StartCommand : SingleCommand
{
    protected override async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Привет, я ParallAI! Пиши /help и я скажу, что умею!");
    }
}