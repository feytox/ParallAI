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
    protected abstract bool ContainsAt<T>(int index, User user);
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
        var buttons = CreateButtons(index, data, user).Chunk(3);

        await bot.EditCallbackMessage(query, info, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private IEnumerable<InlineKeyboardButton> CreateButtons(int index, string[] data, User user)
    {
        yield return InlineKeyboardButton.WithCallbackData("Выбрать", $"{data[0]}:{data[1]}:choose");
        if (!ContainsAt<IImmutableElement>(index, user))
        {
            yield return InlineKeyboardButton.WithCallbackData("Изменить", $"{data[0]}:{data[1]}:edit");
            yield return InlineKeyboardButton.WithCallbackData("Удалить", $"{data[0]}:{data[1]}:remove");
        }
    }

    private Task HandleElementAction(CallbackQuery query, string[] data, ITelegramBotClient bot, User user)
    {
        var index = int.Parse(data[1]);
        return data[2] switch
        {
            "choose" => HandleChoose(query, index, bot, user),
            "edit" => HandleEdit(query, index, bot, user),
            "remove" => HandleRemove(query, index, bot, user),
            _ => throw new ArgumentOutOfRangeException($"Invalid callback: {query.Data}")
        };
    }
}