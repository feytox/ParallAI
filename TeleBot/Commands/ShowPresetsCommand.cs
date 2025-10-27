using Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = AICore.User;

namespace TeleBot.Commands;

[Command("/showpresets", "вывод пользовательских пресетов")]
public class ShowPresetsCommand(IRepository<User, long> users, ILogger<ShowPresetsCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = users.GetOrThrow(userId);

        if (user.Result.Presets.Count == 0)
        {
            await bot.SendMessage(message.Chat, "Ты пока что не добавил пресетов");
            return;
        }
        
        var buttons = user.Result.Presets
            .Select(p => InlineKeyboardButton.WithCallbackData(p.Name, $"preset:{p.Id}"))
            .Chunk(2);

        await bot.SendMessage(message.Chat, "Твои пресеты:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}