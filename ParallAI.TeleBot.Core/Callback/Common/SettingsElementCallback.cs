using ParallAI.Core.Repositories;
using ParallAI.Core.Util;
using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Callback.Common;

public abstract class SettingsElementCallback(IRepository<User, long> users)
    : UserCallbackQuery<SettingsElementArgs>(users)
{
    protected abstract string GetElementInfo(int index, User user);
    protected abstract bool ContainsAt<T>(int index, User user);
    protected abstract Task HandleChoose(CallbackQuery query, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleEdit(CallbackQuery query, int index, ITelegramBotClient bot, User user);
    protected abstract Task HandleRemove(CallbackQuery query, int index, ITelegramBotClient bot, User user);
    protected abstract InlineKeyboardButton? CreateBackButton(string text);

    protected override async Task Handle(
        CallbackQuery query, CallbackData<SettingsElementArgs> data, ITelegramBotClient bot, User user)
    {
        if (data.Args.IsAction)
            await HandleElementAction(query, data, bot, user);
        else
            await HandleElementChoice(query, data, bot, user);
    }

    private async Task HandleElementChoice(
        CallbackQuery query, CallbackData<SettingsElementArgs> data, ITelegramBotClient bot, User user)
    {
        var index = data.Args.Index;
        if (index == -1)
        {
            await HandleEdit(query, index, bot, user);
            return;
        }

        var info = GetElementInfo(index, user);
        var buttons = CreateButtons(index, data, user).Chunk(3);

        await bot.EditCallbackMessage(query, info, replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private IEnumerable<InlineKeyboardButton> CreateButtons(int index, CallbackData<SettingsElementArgs> data, User user)
    {
        yield return InlineKeyboardButton.WithCallbackData(
            "Выбрать", $"{data.Key}:{data.Args.Index}:choose");
        if (!ContainsAt<IImmutableElement>(index, user))
        {
            yield return InlineKeyboardButton.WithCallbackData(
                "Изменить", $"{data.Key}:{data.Args.Index}:edit");
            yield return InlineKeyboardButton.WithCallbackData(
                "Удалить", $"{data.Key}:{data.Args.Index}:remove");
        }

        var backButton = CreateBackButton("Назад");
        if (backButton is not null)
            yield return backButton;
    }

    private Task HandleElementAction(
        CallbackQuery query, CallbackData<SettingsElementArgs> data, ITelegramBotClient bot, User user)
    {
        var index = data.Args.Index;
        return data.Args.Action switch
        {
            SettingsElementAction.Choose => HandleChoose(query, index, bot, user),
            SettingsElementAction.Edit => HandleEdit(query, index, bot, user),
            SettingsElementAction.Remove => HandleRemove(query, index, bot, user),
            _ => throw new ArgumentOutOfRangeException($"Invalid callback: {query.Data}")
        };
    }
}