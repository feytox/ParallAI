using ParallAI.Core.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.StateActions;

public interface IStateAction
{
    bool CanHandle(UserState state);

    Task Execute(UserState state, Message message, ITelegramBotClient bot, User user);
}