using ParallAI.Core.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class SettingsHandler<TState, TResult>(params SettingsPart<TState>[] parts)
    where TState : SettingsState
{
    protected abstract Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot, User user);
    protected abstract TResult CreateResult(TState state);
    protected abstract void SaveResult(TResult result);
    
    public async Task<bool> ActivatePart(TState state, int partIndex, ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (partIndex >= parts.Length)
            throw new IndexOutOfRangeException("Invalid settings part index");

        var selectedPart = parts[partIndex];
        state.CurrentPart = partIndex;
        
        var nextState = await selectedPart.ActivatePart(state, chatId, bot, user);
        if (nextState is not null)
            user.StateMachine.Push(nextState);
        
        return true;
    }

    public async Task HandleMessage(TState state, Message message, ITelegramBotClient bot, User user)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is IEmbeddedPart<TState> embeddedPart)
        {
            var wasHandled = await embeddedPart.HandleMessage(state, message, bot);
            if (!wasHandled)
                return;
        }
        
        await SendPartsList(state, message.Chat, bot, user);
    }
    
    public async Task<bool> SavePartResult(TState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (state.PrevState is null)
            return false;

        var currentPart = GetCurrentPart(state);
        if (currentPart is null)
            throw new NullReferenceException("Previous state is not null, but current part is null");

        currentPart.SaveToState(state, state.PrevState);
        await SendPartsList(state, chatId, bot, user);
        return true;
    }

    private SettingsPart<TState>? GetCurrentPart(TState state)
    {
        return state.CurrentPart is null ? null : parts[state.CurrentPart.Value];
    }
}