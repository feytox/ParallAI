using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.StateActions.Common;

public interface IStepStateAction<in TState, out TStep> where TState : SequentialState<TStep>
{
    TStep StateStep { get; }
    Task<bool> Execute(TState state, Message message, ITelegramBotClient bot, User user);
}