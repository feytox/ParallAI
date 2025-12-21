using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Settings;

public interface IMessageHandlerPart<in TState> where TState : SettingsState
{
    Task<bool> HandleMessage(TState state, Message message, ITelegramBotClient bot);
}