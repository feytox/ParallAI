using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🤖 Модели", MenuOrder.Models)]
[Command("/models", "список и конфигурация моделей")]
public class ModelsCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var buttons = user.UserModels
            .Select((model, i) => ModelCallback.Create(model.DisplayName, i.ToString()))
            .Chunk(2)
            .Append([ModelCallback.Create("Создать", "-1")]);

        await bot.SendMessage(message.Chat, "Ваши модели:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}