using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class StandardSettingsHandler<TState>(string tag, Action<SettingsPartsBuilder<TState>> partsProvider)
    : SettingsHandler<TState>(tag, partsProvider)
    where TState : SettingsState
{
    protected abstract string GetPartsMessage(TState state);
    protected abstract Task SaveSettingsToUser(TState state, ChatId chatId, ITelegramBotClient bot, User user);
    
    
    protected override async Task SendPartsList(TState state, ChatId chatId, ITelegramBotClient bot)
    {
        var isAllValid = parts
            .OfType<IValidatablePart<TState>>()
            .All(p => p.Validate(state));
        
        var partsButtons = CreatePartButtons(state).Chunk(2);
        
        var bottomButtons = new List<InlineKeyboardButton>();
        if (isAllValid)
            bottomButtons.Add(InlineKeyboardButton.WithCallbackData("𒀱Сохранить", $"{Tag}:c"));
        bottomButtons.Add(CancelCallback.CreateButton("🔙Выйти без сохранения"));
        
        var allButtons = partsButtons.Append(bottomButtons.ToArray());

        var message = GetPartsMessage(state);
        await bot.SendMessage(chatId, message, replyMarkup: new InlineKeyboardMarkup(allButtons));
    }

    public override async Task FinalizeSettings(TState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        await SaveSettingsToUser(state, chatId, bot, user);
        user.StateMachine.Pop();
    }
}