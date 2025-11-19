using Microsoft.Extensions.Logging;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Example.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands.Presets;

[Command("/showpresets", "вывод пользовательских пресетов")]
public class ShowPresetsCommand(IRepository<User, long> users, ILogger<TestCommand> logger) 
    : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.UserPresets
            .Select(p => InlineKeyboardButton.WithCallbackData(p.Name, $"preset:{p.Id}"))
            .Chunk(2);

        await bot.SendMessage(message.Chat, "Твои пресеты:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}