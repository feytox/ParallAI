#region

using AICore.Repositories;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot;

public interface ICommand
{
    Task Execute(Message message, ITelegramBotClient bot);
}

public abstract class UserCommand(IRepository<User, long> userRepository) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var user = await userRepository.GetOrCreate(message.Chat.Id);
        await Execute(message, bot, user);
        await userRepository.Update(user);
    }
    
    public abstract Task Execute(Message message, ITelegramBotClient bot, User user);
}