using Infrastructure;
using Microsoft.Extensions.Logging;
using TeleBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.User;


namespace TeleBot.Commands;

[Command("/test", "тестовая команда для тестов")]
public class TestCommand(IRepository<User, long> users, ILogger<TestCommand> logger) : SingleCommand
{
    protected override async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        if (await users.TryGetById(userId, out _))
        {
            await bot.SendMessage(message.Chat, $"Юзер {userId} уже в бд");
            logger.LogInformation($"Юзер {userId} уже в бд");
            return;
        }
        
        var user = new User(userId);
        await users.AddOrThrow(user);
        await bot.SendMessage(message.Chat, $"Юзер {userId} добавлен в бд");
        logger.LogInformation($"Юзер {userId} добавлен в бд");
    }
}