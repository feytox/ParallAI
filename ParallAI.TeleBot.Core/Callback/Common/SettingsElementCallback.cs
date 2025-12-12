using ParallAI.Core.Entities.DefaultPresets;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class SettingsElementCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    protected abstract string GetElementInfo(int index, User user);
    protected abstract object GetElement(int index, User user);
    protected abstract Task HandleChoose(CallbackQuery query, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleEdit(CallbackQuery query, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleRemove(CallbackQuery query, int index, ITelegramBotClient bot, User user);

    protected override async Task Handle(CallbackQuery query, ITelegramBotClient bot, User user)
    {
        var data = query.Data!.Split(':');
        switch (data.Length)
        {
            case 2:
                await HandleElementChoice(query, data, bot, user);
                break;
            case 3:
                await HandleElementAction(query, data, bot, user);
                break;
            default:
                throw new ArgumentException($"Invalid callback: {query.Data}");
        }
    }

    private async Task HandleElementChoice(CallbackQuery query, string[] data, ITelegramBotClient bot, User user)
    {
        var index = int.Parse(data[1]);
        if (index == -1)
        {
            await HandleEdit(query, index, bot, user);
            return;
        }

        var info = GetElementInfo(index, user);
        var buttons = new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("Выбрать", $"{data[0]}:{data[1]}:c"),
        };
        if (GetElement(index, user) is not IImmutableElement)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData("Изменить", $"{data[0]}:{data[1]}:e"));
            buttons.Add(InlineKeyboardButton.WithCallbackData("Удалить", $"{data[0]}:{data[1]}:r"));
        }

        await bot.EditCallbackMessage(query, info, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private Task HandleElementAction(CallbackQuery query, string[] data, ITelegramBotClient bot, User user)
    {
        var index = int.Parse(data[1]);
        return data[2] switch
        {
            "c" => HandleChoose(query, index, bot, user),
            "e" => HandleEdit(query, index, bot, user),
            "r" => HandleRemove(query, index, bot, user),
            _ => throw new ArgumentOutOfRangeException($"Invalid callback: {query.Data}")
        };
    }
}