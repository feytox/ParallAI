using AICore.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Commands.Common;

public abstract class UserCommand(IRepository<User, long> users) : ICommand
{
    protected abstract Task Execute(Message message, ITelegramBotClient bot, User user);

    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var user = await users.GetOrCreate(message.Chat.Id);
        await Execute(message, bot, user);
        await users.Update(user);
    }
}