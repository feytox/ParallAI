using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🛠️ Текущая конфигурация", MenuOrder.Config)]
[Command("/config", "текущая конфигурация")]
public class ConfigCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(ChatId chatId, ITelegramBotClient bot, User user)
    {
        var modelName = user.ChosenModel?.DisplayName;
        var presetName = user.ChosenPreset?.Name;

        var buttons = new[]
        {
            CommandCallback.Create<ModelsCommand>("Модели"),
            CommandCallback.Create<ModelsCommand>("Пресеты")
        };

        await bot.SendMessage(chatId,
            "<b>Модель</b> — " + (modelName ?? "не выбрана") +
            "\n<b>Пресет</b> — " + (presetName ?? "не выбран"),
            parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}