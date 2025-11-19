using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Callback;

public interface ICallbackQuery
{
    Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot);
}