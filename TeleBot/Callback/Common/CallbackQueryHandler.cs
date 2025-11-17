using System.Reflection;
using AICore.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Callback.Common;

public class CallbackQueryHandler
{
    private readonly Dictionary<string, ICallbackQuery> _callbackQueriesDict;

    public CallbackQueryHandler(IEnumerable<ICallbackQuery> callbackQueries)
    {
        _callbackQueriesDict = callbackQueries
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
        if (_callbackQueriesDict.TryGetValue(callbackQueryKey, out var callbackQueryObject))
            await callbackQueryObject.Handle(callbackQuery, bot);
        else
            await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}