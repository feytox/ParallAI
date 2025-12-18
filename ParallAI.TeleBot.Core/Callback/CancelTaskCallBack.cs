using ParallAI.Core.Repositories;
using ParallAI.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

[CallbackQuery(Tag)]
public class CancelTaskCallBack(IRepository<User, long> users, CancelTokenSourceStorage storage) : UserCallbackQuery(users)
{
    public const string Tag = "cancelTask";
    
    protected override Task Handle(CallbackQuery query, CallbackData data, ITelegramBotClient bot, User user)
    {
        if (!Guid.TryParse(data.Args[0], out var ctsId))
            throw new ArgumentException("Invalid callback data");
        var cts = storage.GetSource(ctsId);
        cts.Cancel();
        return Task.CompletedTask;
    }
}