using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Core.Commands;

[Command("/cancel", "выйти из пошаговой команды", HighPriority = true)]
public class CancelCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        await CancelHelper.Cancel(message.Chat, null, false, bot, user);
    }
}