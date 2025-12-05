using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.Providers;

public abstract class ProviderChooseCallback<TProviderSettingsState>(IRepository<User, long> users)
    : UserCallbackQuery(users) 
    where TProviderSettingsState : ProviderSettingsState, new()
{
    protected abstract TProviderSettingsState ToSettingsState(AiProvider provider);

    protected override Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        if (user.StateMachine.Current is not ModelSettingsState modelSettings)
            throw new InvalidOperationException("Incorrect current state");

        var aiProvider = modelSettings.Provider;
        var state = aiProvider is null ? new TProviderSettingsState() : ToSettingsState(aiProvider);
        user.StateMachine.Push(state);
        
        return Task.CompletedTask;
    }
}