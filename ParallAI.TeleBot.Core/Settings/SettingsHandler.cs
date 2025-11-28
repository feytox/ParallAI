using ParallAI.Core.States.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

// TODO: add validation
public abstract class SettingsHandler<TState> where TState : SettingsState
{
    protected abstract string GetPartsMessage(TState state);
    protected abstract void SaveResult(TState state, User user);

    private readonly string callbackId;
    private readonly SettingsPart<TState>[] parts;

    protected SettingsHandler(string callbackId, SettingsPart<TState>[] parts)
    {
        this.callbackId = callbackId;
        this.parts = parts;
    }

    protected SettingsHandler(string callbackId, Action<SettingsPartsBuilder<TState>> partsProvider)
    {
        this.callbackId = callbackId;
        
        var builder = new SettingsPartsBuilder<TState>();
        partsProvider(builder);
        parts = builder.Build();
    }

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
        
        await SendPartsList(state, message.Chat, bot);
    }
    
    public async Task<bool> SavePartResult(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        if (state.PrevState is null)
            return false;

        var currentPart = GetCurrentPart(state);
        if (currentPart is null)
            throw new NullReferenceException("Previous state is not null, but current part is null");

        currentPart.SaveToState(state, state.PrevState);
        await SendPartsList(state, chatId, bot);
        return true;
    }
    
    public async Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        var buttons = parts
            .Select((part, i) => InlineKeyboardButton.WithCallbackData(part.Name, $"{callbackId}:{i}"))
            .Chunk(2);
        var message = GetPartsMessage(state);

        await bot.SendMessage(chatId, message, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private SettingsPart<TState>? GetCurrentPart(TState state)
    {
        return state.CurrentPart is null ? null : parts[state.CurrentPart.Value];
    }
}