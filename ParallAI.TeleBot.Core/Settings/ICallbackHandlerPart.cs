using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public interface ICallbackHandlerPart<in TState> where TState : SettingsState
{
    Task<bool> HandleCallBack(TState state, CallbackQuery callback, ITelegramBotClient bot, User user);
}