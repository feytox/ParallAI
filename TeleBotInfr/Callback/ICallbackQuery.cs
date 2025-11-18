using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBotInfr.Callback;

public interface ICallbackQuery
{
    Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot);
}