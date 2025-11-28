using ParallAI.Core.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Settings;

public interface IEmbeddedPart<in TState> where TState : SettingsState
{
    Task<bool> HandleMessage(TState state, Message message, ITelegramBotClient bot);
}