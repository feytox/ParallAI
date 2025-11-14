using AICore.Entities;
using AICore.Repositories;
using Microsoft.Extensions.Logging;
using TeleBot.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = AICore.Entities.User;

namespace TeleBot.Example.Commands;

// временная команда пресетов
[Command("/showpresets", "вывод пользовательских пресетов")]
public class ShowPresetsCommand(IRepository<User, long> users, ILogger<TestCommand> logger) 
    : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var presets = new List<Preset> { new(Guid.NewGuid(), "ask"), new(Guid.NewGuid(), "terver") };

        var buttons = presets
            .Select(p => InlineKeyboardButton.WithCallbackData(p.Name, $"preset:{p.Id}"))
            .Chunk(2);

        await bot.SendMessage(message.Chat, "Твои пресеты:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}