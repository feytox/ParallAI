#region

using AICore.Entities;
using AICore.Repositories;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion


namespace TeleBot.Commands;

[Command("/test", "тестовая команда для тестов")]
public class TestCommand(IRepository<User, long> userRepository, ILogger<TestCommand> logger) : UserCommand(userRepository)
{
    public override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var userId = message.From!.Id;
        var model = new AiModel(Guid.NewGuid(), "gemini");

        user.AddModel(model);
        
        await bot.SendMessage(message.Chat, $"Модель ({user.Models.Count}) {model} добавлена к {userId}");
    }
}