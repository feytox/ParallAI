using ParallAI.Core.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public interface IStepStateAction<in TState, out TStep> where TState : SequentialState<TStep>
{
    TStep StateStep { get; }
    Task<bool> Execute(TState state, Message message, ITelegramBotClient bot, User user);
}