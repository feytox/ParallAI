using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🛠️ Текущая конфигурация", MenuOrder.Config)]
[Command("/config", "текущая конфигурация")]
public class ConfigCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var modelName = user.ChosenModel?.DisplayName;
        var presetName = user.ChosenPreset?.Name;

        await bot.SendMessage(message.Chat,
            "<b>Модель</b> (/models) — " + (modelName ?? "не выбрана") + 
            "\n<b>Пресет</b> (/presets) — " + (presetName ?? "не выбран"),
            ParseMode.Html);
    }
}