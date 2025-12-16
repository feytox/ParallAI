using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class TypedCallbackQuery<TArgs> : ICallbackQuery
    where TArgs : ICallbackArgs, new()
{
    protected abstract Task Handle(CallbackQuery query, CallbackData<TArgs> data, ITelegramBotClient bot);

    public async Task Handle(CallbackQuery query, CallbackData data, ITelegramBotClient bot)
    {
        var dataArgs = new CallbackData<TArgs>(data);
        await Handle(query, dataArgs, bot);
    }
}