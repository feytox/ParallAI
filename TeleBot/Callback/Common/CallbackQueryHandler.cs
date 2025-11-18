using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Callback.Common;

public class CallbackQueryHandler
{
    private readonly Dictionary<string, ICallbackQuery> callbackQueries;

    public CallbackQueryHandler(IEnumerable<ICallbackQuery> callbackQueries)
    {
        this.callbackQueries = callbackQueries
            .Select(cbq => (cbq, attr: cbq.GetType().GetCustomAttribute<CallbackQueryAttribute>()))
            .Where(t => t.attr is not null)
            .ToDictionary(t => t.attr!.Key, t => t.cbq, StringComparer.OrdinalIgnoreCase);
    }

    public async Task HandleCallbackQuery(CallbackQuery callbackQuery, ITelegramBotClient bot)
    {
        if (string.IsNullOrEmpty(callbackQuery.Data))
        {
            await bot.AnswerCallbackQuery(callbackQuery.Id);
            return;
        }
        
        var callbackQueryKey = callbackQuery.Data.Split(':')[0];
        if (callbackQueries.TryGetValue(callbackQueryKey, out var callbackQueryObject))
            await callbackQueryObject.Handle(callbackQuery, bot);
        else
            await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}