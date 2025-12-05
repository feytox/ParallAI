using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

// TODO: add validation
public abstract class SettingsHandler<TState> where TState : SettingsState
{
    protected abstract Task SaveSettingsToUser(TState state, ChatId chatId, ITelegramBotClient bot, User user);
    protected abstract string GetPartsMessage(TState state);

    private readonly string tag;
    private readonly SettingsPart<TState>[] parts;

    protected SettingsHandler(string tag, SettingsPart<TState>[] parts)
    {
        this.tag = tag;
        this.parts = parts;
    }

    protected SettingsHandler(string tag, Action<SettingsPartsBuilder<TState>> partsProvider)
    {
        this.tag = tag;

        var builder = new SettingsPartsBuilder<TState>();
        partsProvider(builder);
        parts = builder.Build();
    }

    public async Task ActivatePart(TState state, int partIndex, ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (partIndex >= parts.Length)
            throw new IndexOutOfRangeException("Invalid settings part index");

        var selectedPart = parts[partIndex];
        state.CurrentPart = partIndex;

        var nextState = await selectedPart.ActivatePart(state, chatId, bot, user);
        if (nextState is not null)
            user.StateMachine.Push(nextState);
    }

    public async Task HandleMessage(TState state, Message message, ITelegramBotClient bot, User user)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is IMessageHandlerPart<TState> embeddedPart)
        {
            var wasHandled = await embeddedPart.HandleMessage(state, message, bot);
            if (!wasHandled)
                return;
        }

        await SendPartsList(state, message.Chat, bot);
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
        return true;
    }

    private void TrySavePartResult(TState state)
    {
        var currentPart = GetCurrentPart(state);
        if (currentPart is null)
            throw new NullReferenceException("Previous state is not null, but current part is null");

        if (currentPart is ICanSavePart<TState> part) 
            part.SaveToState(state, state.PrevState!);
    }

    public async Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        var buttons = parts
            .Select((part, i) => InlineKeyboardButton.WithCallbackData(part.Name, $"{tag}:{i}"))
            .Chunk(2)
            .Append([InlineKeyboardButton.WithCallbackData("Сохранить", $"{tag}:c")])
            .Append([CancelCallback.CreateButton("Выйти без сохранения")]);
        var message = GetPartsMessage(state);

        await bot.SendMessage(chatId, message, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    public async Task SaveSettings(TState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        await SaveSettingsToUser(state, chatId, bot, user);
        user.StateMachine.Pop();
    }

    private SettingsPart<TState>? GetCurrentPart(TState state)
    {
        return state.CurrentPart is null ? null : parts[state.CurrentPart.Value];
    }
}