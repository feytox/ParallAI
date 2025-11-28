using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class MainMenuStateAction : StateAction<MainMenuState>
{
    protected override Task<bool> Execute(MainMenuState state, Message message, ITelegramBotClient bot, User user)
    {
        return Task.FromResult(false);
    }

    protected override async Task<bool> ExecuteAfter(MainMenuState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        if (!state.Reactivated)
            return false;

        await bot.SendMessage(chatId, "Привет, я параллаич! (плейсхолдер)");
        state.Reactivated = false;
        return true;
    }
}