using Microsoft.Extensions.Logging;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Example.Commands;

[Command("/test", "тестовая команда для тестов")]
public class TestCommand(IRepository<User, long> users, ILogger<TestCommand> logger) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var userId = message.From!.Id;
        var provider = new GeminiProvider("TOKEN");
        var model = new AiModel(Guid.NewGuid(), "gemini-2.5-flash", "Gemini 2.5 Flash", provider);

        user.AddModel(model);
        await bot.SendMessage(message.Chat, $"Модель ({user.UserModels.Count}) {model} добавлена к {userId}");
    }
}