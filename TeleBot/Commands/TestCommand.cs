#region

using AICore.Entities;
using AICore.Repositories;
using AICore.ValueTypes;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion


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
        var provider = new GeminiProvider("TOKEN");
        var model = new AiModel(Guid.NewGuid(), "gemini-2.5-pro", "Gemini 2.5 Pro", provider);

        user.AddModel(model);
        await users.Update(user);
        
        await bot.SendMessage(message.Chat, $"Модель {model} добавлена к {userId}");
    }
}