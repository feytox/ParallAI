using AICore.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBotInfr.Callback;

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