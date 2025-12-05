using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Core.Util;

public class CancelHelper
{
    public static async Task Cancel(ChatId chatId, Message? messageToDelete, ITelegramBotClient bot, User user)
    {
        if (messageToDelete is not null)
            await bot.DeleteMessage(chatId, messageToDelete.Id);

        if (!user.StateMachine.TryPop(cancelled: true))
        {
            await bot.SendMessage(chatId, "Сейчас нет команды, которую можно отменить");
            return;
        }

        await bot.SendMessage(chatId, "Команда отменена");
    }
}