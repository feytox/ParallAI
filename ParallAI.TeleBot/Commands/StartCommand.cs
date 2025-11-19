using Microsoft.Extensions.Logging;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Example.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[Command("/start", "стартовая команда")]
public class StartCommand(IRepository<User, long> users, ILogger<TestCommand> logger) 
    : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(message.Chat, "Привет, я ParallAI! Пиши /help и я скажу, что умею!");
    }
}