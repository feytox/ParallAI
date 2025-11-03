#region

using AICore.Repositories;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot.Commands;

[Command("/start", "стартовая команда")]
public class StartCommand(IRepository<User, long> userRepository, ILogger<TestCommand> logger) 
    : UserCommand(userRepository)
{
    public override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(message.Chat, "Привет, я ParallAI! Пиши /help и я скажу, что умею!");
    }
}