using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("⚖️ Режим сравнения", MenuOrder.Compare)]
[Command("/compare", "глубокое сравнение запроса")]
public class CompareCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (!await ValidateModelsCount(chatId, bot, user) || !await ValidatePresetsCount(chatId, bot, user))
            return;

        var buttons = user.ComparisonDates
            .Reverse()
            .Select(time => time.ToUniversalTime().ToString("yyyy/MM/dd HH:mm z"))
            .Select((text, i) => CompareCallback.CreateButton(text, $"{i}"))
            .Append(CompareCallback.CreateButton("+", "+"))
            .Chunk(1);

        await bot.SendMessage(chatId, "Выберите предыдущий конфиг сравнения или создайте новый:",
            replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private static async Task<bool> ValidateModelsCount(ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (user.UserModels.Count > 0)
            return true;

        var markup = new InlineKeyboardMarkup(CommandCallback.Create<ModelsCommand>("Настроить"));
        await bot.SendMessage(chatId, "У вас 0 настроенных моделей", replyMarkup: markup);
        return false;
    }

    private static async Task<bool> ValidatePresetsCount(ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (user.UserPresets.Count > 0)
            return true;

        var markup = new InlineKeyboardMarkup(CommandCallback.Create<PresetsCommand>("Настроить"));
        await bot.SendMessage(chatId, "У вас 0 настроенных пресетов", replyMarkup: markup);
        return false;
    }
}