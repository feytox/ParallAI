using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class UserCallbackQuery(IRepository<User, long> users) : ICallbackQuery
{
    protected IRepository<User, long> Users { get; } = users;

    protected abstract Task Handle(CallbackQuery query, CallbackData data, ITelegramBotClient bot, User user);

    public async Task Handle(CallbackQuery query, CallbackData data, ITelegramBotClient bot)
    {
        var user = await Users.GetOrCreate(query.Message!.Chat.Id);
        await Handle(query, data, bot, user);
        await Users.Update(user);
    }
}

public abstract class UserCallbackQuery<TArgs>(IRepository<User, long> users) : UserCallbackQuery(users)
    where TArgs : ICallbackArgs<TArgs>
{
    protected abstract Task Handle(CallbackQuery query, CallbackData<TArgs> data, ITelegramBotClient bot, User user);

    protected override async Task Handle(CallbackQuery query, CallbackData data, ITelegramBotClient bot, User user)
    {
        var dataArgs = new CallbackData<TArgs>(data);
        await Handle(query, dataArgs, bot, user);
    }
}