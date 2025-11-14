using AICore.Repositories;
using TeleBot.Commands.Common;
using TeleBot.Example.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.Commands;

[Command("/request", "тестовая команда для запросов к модели")]
public class RequestCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        user.StateMachine.Set(new RequestState());
        await bot.SendMessage(message.Chat, "Введи запрос. Также можешь прикрепить файл");
    }
}