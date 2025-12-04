using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Example.Commands;

[Command("/request", "тестовая команда для запросов к модели")]
public class RequestCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        user.StateMachine.Push(new RequestState());
        await bot.SendMessage(message.Chat, "Введи запрос. Также можешь прикрепить файл",
            replyMarkup: CancelCallback.CreateMarkup("Отменить"));
    }
}