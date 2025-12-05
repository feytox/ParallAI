using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class SettingsPartCallback<TState, THandler>(IRepository<User, long> users, THandler handler)
    : UserCallbackQuery(users)
    where TState : SettingsState
    where THandler : SettingsHandler<TState>
{
    protected readonly THandler Handler = handler;
    
    protected abstract Task<bool> HandleDataContent(TState state, ChatId chatId, string content, 
        ITelegramBotClient bot, User user);
    
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var data = callbackQuery.Data!.Split(':');
        if (data.Length != 2)
            throw new ArgumentException($"Invalid callback data: {callbackQuery.Data}");

        var currentState = user.StateMachine.Current;
        if (currentState is not TState state)
            throw new InvalidOperationException($"{typeof(TState)} callback called for {currentState}");
        
        var content = data[1];
        var chatId = callbackQuery.From.Id;
        if (!await HandleDataContent(state, chatId, content, bot, user))
            await Handler.ActivatePart(state, int.Parse(content), chatId, bot, user);
    }
}