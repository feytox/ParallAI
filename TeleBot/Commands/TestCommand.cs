using AICore;
using AICore.Repositories;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;


namespace TeleBot.Commands;

[Command("/test", "тестовая команда для тестов")]
public class TestCommand(
    IRepository<User, long> users,
    ILogger<TestCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = await users.GetOrCreate(userId);
        var model = new AiModel(Guid.NewGuid(), "gemini");

        user.AddModel(model);
        await users.Update(user);
        
        await bot.SendMessage(message.Chat, $"Модель ({user.Models.Count}) {model} добавлена к {userId}");
    }
}