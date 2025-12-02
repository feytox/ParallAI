using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(Tag)]
public class CancelCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    private const string Tag = "cancel";
    
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        await CancelHelper.Cancel(callbackQuery.From.Id, callbackQuery.Message, bot, user);
    }

    public static InlineKeyboardMarkup CreateMarkup(string text) => new(CreateButton(text));
    
    public static InlineKeyboardButton CreateButton(string text) => InlineKeyboardButton.WithCallbackData(text, Tag);
}