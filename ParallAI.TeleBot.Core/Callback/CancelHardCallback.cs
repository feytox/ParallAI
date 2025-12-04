using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback;

[CallbackQuery(Tag)]
public class CancelHardCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    private const string Tag = "cancel-hard";
    
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        await CancelHelper.Cancel(callbackQuery.From.Id, callbackQuery.Message, true, bot, user);
    }

    public static InlineKeyboardMarkup CreateMarkup(string text) => new(CreateButton(text));
    
    public static InlineKeyboardButton CreateButton(string text) => InlineKeyboardButton.WithCallbackData(text, Tag);
}