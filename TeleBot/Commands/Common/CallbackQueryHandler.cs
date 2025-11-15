using AICore.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot;

// временно
public class CallbackQueryHandler
{
    public async Task HandleCallbackQuery(CallbackQuery callbackQuery, ITelegramBotClient bot)
    {
        if (string.IsNullOrEmpty(callbackQuery.Data))
        {
            await bot.AnswerCallbackQuery(callbackQuery.Id);
            return;
        }

        var callbackQueryKey = callbackQuery.Data.Split(':')[0];
        var callbackQueryValue = callbackQuery.Data.Split(':')[1];
        switch (callbackQueryKey)
        {
            case "preset":
                await bot.AnswerCallbackQuery(callbackQuery.Id);
                await bot.SendMessage(callbackQuery.Message!.Chat, $"ID пресета: {callbackQueryValue}");
                break;
        }
    }
}