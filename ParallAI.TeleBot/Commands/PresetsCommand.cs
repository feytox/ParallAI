using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static ParallAI.TeleBot.Callback.PresetCallback;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[Command("/presets", "список и конфигурация пресетов")]
public class PresetsCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.UserPresets
            .Select((preset, i) => InlineKeyboardButton.WithCallbackData(preset.Name, $"{Tag}:{i}"))
            .Chunk(2)
            .Append([InlineKeyboardButton.WithCallbackData("Создать", $"{Tag}:-1")]);

        await bot.SendMessage(message.Chat, "Ваши пресеты:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}