using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Callback.Common;

public interface ICallbackQuery
{
    Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot);
}