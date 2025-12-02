using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;
using static ParallAI.TeleBot.Callback.ModelCallback;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🤖 Модели", MenuOrder.Models)]
[Command("/models", "список и конфигурация моделей")]
public class ModelsCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.UserModels
            .Select((model, i) => InlineKeyboardButton.WithCallbackData(model.DisplayName, $"{Tag}:{i}"))
            .Chunk(2)
            .Append([InlineKeyboardButton.WithCallbackData("Создать", $"{Tag}:-1")]);

        await bot.SendMessage(message.Chat, "Ваши модели:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}