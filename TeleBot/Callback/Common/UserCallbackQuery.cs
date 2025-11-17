using AICore.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Callback.Common;

public abstract class UserCallbackQuery(IRepository<User, long> users) : ICallbackQuery
{
    protected abstract Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user);

    public async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot)
    {
        var user = await users.GetOrCreate(callbackQuery.From.Id);
        await Handle(callbackQuery, bot, user);
        await users.Update(user);
    }
}