using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class CompareStateAction(GenerationService genService, MediaGroupCollector groupCollector) 
    : StateAction<CompareState>
{
    protected override async Task<bool> Execute(CompareState state, Message message, ITelegramBotClient bot, User user)
    {
        throw new NotImplementedException();
    }

    protected override async Task<bool> ExecuteAfter(CompareState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(chatId, "Я ещё не реализован, братик :3");
        user.StateMachine.Pop();
        return true;
    }
}