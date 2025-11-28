using ParallAI.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback;

public abstract class SettingsElementCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    protected abstract string GetElementInfo(int index, User user);
    protected abstract Task HandleChoose(CallbackQuery callbackQuery, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleEdit(CallbackQuery callbackQuery, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleRemove(CallbackQuery callbackQuery, int index, ITelegramBotClient bot, User user);

    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var data = callbackQuery.Data!.Split(':');
        switch (data.Length)
        {
            case 2:
                await HandleElementChoice(callbackQuery, data, bot, user);
                break;
            case 3:
                await HandleElementAction(callbackQuery, data, bot, user);
                break;
            default:
                throw new ArgumentException($"Invalid callback: {callbackQuery.Data}");
        }
    }

    private async Task HandleElementChoice(CallbackQuery callbackQuery, string[] data,
        ITelegramBotClient bot, User user)
    {
        var index = int.Parse(data[1]);
        if (index == -1)
        {
            await HandleEdit(callbackQuery, index, bot, user);
            return;
        }

        var info = GetElementInfo(index, user);
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Выбрать", $"{data[0]}:{data[1]}:c"),
            InlineKeyboardButton.WithCallbackData("Изменить", $"{data[0]}:{data[1]}:e"),
            InlineKeyboardButton.WithCallbackData("Удалить", $"{data[0]}:{data[1]}:r")
        };

        await bot.SendMessage(callbackQuery.From.Id, info, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private Task HandleElementAction(CallbackQuery callbackQuery, string[] data, ITelegramBotClient bot, User user)
    {
        var index = int.Parse(data[1]);
        return data[2] switch
        {
            "c" => HandleChoose(callbackQuery, index, bot, user),
            "e" => HandleEdit(callbackQuery, index, bot, user),
            "r" => HandleRemove(callbackQuery, index, bot, user),
            _ => throw new ArgumentOutOfRangeException($"Invalid callback: {callbackQuery.Data}")
        };
    }
}