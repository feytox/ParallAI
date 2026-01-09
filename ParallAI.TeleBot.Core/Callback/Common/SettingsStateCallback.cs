using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class SettingsStateCallback<TState, THandler>(IRepository<User, long> users, THandler handler)
    : UserCallbackQuery<SettingsStateArgs>(users)
    where TState : SettingsState
    where THandler : SettingsHandler<TState>
{
    protected readonly THandler Handler = handler;

    protected abstract Task<ActionResult> HandleDataContent(TState state, CallbackQuery query, string content,
        ITelegramBotClient bot, User user);

    protected override async Task Handle(
        CallbackQuery query, CallbackData<SettingsStateArgs> data, ITelegramBotClient bot, User user)
    {
        var currentState = user.StateMachine.Current;
        if (currentState is not TState state)
            throw new InvalidOperationException($"{typeof(TState)} callback called for {currentState}");

        if (await Handler.HandleCallBack(state, query, bot, user) == ActionResult.Handled)
            return;

        var content = data.Args.Content;
        if (await HandleDataContent(state, query, content, bot, user) == ActionResult.Skipped)
            await Handler.ActivatePart(state, int.Parse(content), query, bot, user);
    }
}