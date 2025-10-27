using Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.User;

namespace TeleBot.Commands;

[Command("/start", "стартовая команда")]
public class StartCommand(IRepository<User, long> users, ILogger<TestCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Привет, я ParallAI! Пиши /help и я скажу, что умею!");
        var userId = message.From!.Id;
        if (await users.TryGetById(userId, out _))
        {
            logger.LogInformation($"Юзер {userId} уже в бд");
            return;
        }
        
        var user = new User(userId);
        await users.AddOrThrow(user);
        logger.LogInformation($"Юзер {userId} добавлен в бд");
    }
}