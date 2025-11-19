using ParallAI.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Commands;

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