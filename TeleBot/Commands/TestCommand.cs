#region

using AICore.Entities;
using AICore.Repositories;
using AICore.ValueTypes;
using Microsoft.Extensions.Logging;
using TeleBot.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion


namespace TeleBot.Commands;

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