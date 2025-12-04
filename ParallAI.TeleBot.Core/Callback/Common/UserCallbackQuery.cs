using ParallAI.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class UserCallbackQuery(IRepository<User, long> users) : ICallbackQuery
{
    protected IRepository<User, long> Users { get; } = users;
    
    protected abstract Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user);

    public async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot)
    {
        var user = await Users.GetOrCreate(callbackQuery.From.Id);
        await Handle(callbackQuery, bot, user);
        await Users.Update(user);
    }
}