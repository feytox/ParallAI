using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback;

public class SettingsPartCallback<TState>(IRepository<User, long> users, SettingsHandler<TState> handler)
    : UserCallbackQuery(users)
    where TState : SettingsState
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var data = callbackQuery.Data!.Split(':');
        if (data.Length != 2)
            throw new ArgumentException($"Invalid callback data: {callbackQuery.Data}");

        var currentState = user.StateMachine.Current;
        if (currentState is not TState state)
            throw new InvalidOperationException($"{typeof(TState)} callback called for {currentState}");

        var chatId = callbackQuery.From.Id;
        if (data[1] == "c")
            await handler.SaveSettings(state, chatId, bot, user);
        else
            await handler.ActivatePart(state, int.Parse(data[1]), chatId, bot, user);
    }
}