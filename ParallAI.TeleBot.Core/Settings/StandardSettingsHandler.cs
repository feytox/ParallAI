using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Core.Settings;

public abstract class StandardSettingsHandler<TState>(string tag, Action<SettingsPartsBuilder<TState>> partsProvider)
    : SettingsHandler<TState>(tag, partsProvider)
    where TState : SettingsState
{
    protected abstract string GetPartsMessage(TState state);
    
    protected override async Task SendPartsList(TState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot)
    {
        var isAllValid = Parts
            .OfType<IValidatablePart<TState>>()
            .All(p => p.Validate(state));
        
        var partsButtons = CreatePartButtons(state).Chunk(2);
        
        var bottomButtons = new List<InlineKeyboardButton>();
        if (isAllValid)
            bottomButtons.Add(InlineKeyboardButton.WithCallbackData("💾 Сохранить", $"{Tag}:c"));
        bottomButtons.Add(CancelCallback.CreateButton("🔙 Выйти без сохранения"));
        
        var allButtons = partsButtons.Append(bottomButtons.ToArray());
        if (prevMessage is not null)
            await bot.DeleteMessageOptional(chatId, prevMessage.Id);
        
        var message = GetPartsMessage(state);
        await bot.SendMessage(chatId, message, replyMarkup: new InlineKeyboardMarkup(allButtons));
    }
}