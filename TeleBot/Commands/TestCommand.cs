using Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.User;


namespace TeleBot.Commands;

[Command("/test", "тестовая команда для тестов")]
public class TestCommand(IRepository<User, long> users, ILogger<TestCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = new User(userId);
        await users.Add(user);
        await bot.SendMessage(message.Chat, $"Юзер {userId} добавлен в бд");
        logger.LogInformation($"Юзер {userId} добавлен в бд");
    }
}