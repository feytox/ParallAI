using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("💾 Пресеты", MenuOrder.Presets)]
[Command("/presets", "список и конфигурация пресетов")]
public class PresetsCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.UserPresets
            .Select((preset, i) => PresetCallback.Create(preset.Name, i.ToString()))
            .Chunk(2)
            .Append([PresetCallback.Create("Создать", "-1")]);

        await bot.SendMessage(message.Chat, "Ваши пресеты:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}