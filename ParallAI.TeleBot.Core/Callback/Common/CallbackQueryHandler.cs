using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Callback.Common;

public class CallbackQueryHandler
{
    private readonly Dictionary<string, ICallbackQuery> callbackQueries;

    public CallbackQueryHandler(IEnumerable<(ICallbackQuery callback, CallbackQueryAttribute attribute)> callbacks)
    {
        callbackQueries = callbacks
            .ToDictionary(t => t.attribute.Key, t => t.callback, StringComparer.OrdinalIgnoreCase);
    }

    public async Task HandleCallbackQuery(CallbackQuery callbackQuery, ITelegramBotClient bot)
    {
        if (string.IsNullOrEmpty(callbackQuery.Data))
        {
            await bot.AnswerCallbackQuery(callbackQuery.Id);
            return;
        }
        
        var data = new CallbackData(callbackQuery.Data);
        if (callbackQueries.TryGetValue(data.Key, out var callbackQueryObject))
            await callbackQueryObject.Handle(callbackQuery, data, bot);
        
        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}