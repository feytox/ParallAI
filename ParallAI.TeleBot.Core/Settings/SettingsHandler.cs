using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class SettingsHandler<TState> where TState : SettingsState
{
    public abstract Task FinalizeSettings(TState state, ChatId chatId, ITelegramBotClient bot, User user);
    protected abstract Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot);

    protected readonly string Tag;
    protected readonly SettingsPart<TState>[] Parts;
    
    protected SettingsHandler(string tag, Action<SettingsPartsBuilder<TState>> partsProvider)
    {
        Tag = tag;

        var builder = new SettingsPartsBuilder<TState>();
        partsProvider(builder);
        Parts = builder.Build();
    }

    public async Task ActivatePart(TState state, int partIndex, ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (partIndex >= Parts.Length)
            throw new IndexOutOfRangeException("Invalid settings part index");

        var selectedPart = Parts[partIndex];
        state.CurrentPart = partIndex;

        var nextState = await selectedPart.ActivatePart(state, chatId, bot, user);
        if (nextState is not null)
            user.StateMachine.Push(nextState);
    }

    public async Task HandleMessage(TState state, Message message, ITelegramBotClient bot, User user)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is IMessageHandlerPart<TState> embeddedPart)
            if (!await embeddedPart.HandleMessage(state, message, bot))
                return;

        state.Reactivated = true;
    }

    public async Task<bool> HandleCallBack(TState state, CallbackQuery callback, ITelegramBotClient bot, User user)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is not ICallbackHandlerPart<TState> callbackHandler)
            return false;

        if (!await callbackHandler.HandleCallBack(state, callback, bot, user))
            return false;

        state.Reactivated = true;
        return true;
    }

    public async Task<bool> ExecuteAfter(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        if (!state.Reactivated)
            return false;

        if (state.PrevState is not null)
        {
            TrySavePartResult(state);
            state.RejectPrevState();
        }

        await SendPartsList(state, chatId, bot);
        state.Reactivated = false;
        state.CurrentPart = null;
        return true;
    }

    protected IEnumerable<InlineKeyboardButton> CreatePartButtons(TState state)
    {
        return Parts.Select((part, i) =>
        {
            var icon = "";
            
            if (part is IValidatablePart<TState> validatable)
                icon = validatable.Validate(state) ? "✅" : "";

            return InlineKeyboardButton.WithCallbackData(
                $"{icon}{part.Name}", 
                $"{Tag}:{i}"
            );
        });
    }

    private void TrySavePartResult(TState state)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is null)
            throw new NullReferenceException("Previous state is not null, but current part is null");

        if (currentPart is ICanSavePart<TState> part)
            part.SaveToState(state, state.PrevState!);
    }

    private SettingsPart<TState>? GetCurrentPart(TState state)
    {
        return state.CurrentPart is null ? null : Parts[state.CurrentPart.Value];
    }
}