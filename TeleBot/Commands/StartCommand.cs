#region

using AICore.Repositories;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot.Commands;

[Command("/start", "стартовая команда")]
public class StartCommand(IRepository<User, long> users, ILogger<TestCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(message.Chat, "Привет, я ParallAI! Пиши /help и я скажу, что умею!");
    }
}