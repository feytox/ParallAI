using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Settings;

public interface IEmbeddedCallBackPart<in TState> where TState : SettingsState
{
    Task<bool> HandleCallBack(TState state, CallbackQuery callback, ITelegramBotClient bot);
}