using ParallAI.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Commands.Common;

public abstract class UserCommand(IRepository<User, long> users) : ICommand
{
    protected abstract Task Execute(ChatId chatId, ITelegramBotClient bot, User user);

    public async Task Execute(ChatId chatId, long userId, ITelegramBotClient bot)
    {
        var user = await users.GetOrCreate(userId);
        await Execute(chatId, bot, user);
        await users.Update(user);
    }
}