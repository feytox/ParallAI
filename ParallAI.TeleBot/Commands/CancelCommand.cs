using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Commands;

[Command("/cancel", "выйти из пошаговой команды", HighPriority = true)]
public class CancelCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        try
        {
            user.StateMachine.Pop();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Unable to pop the default UserState."))
        {
            await bot.SendMessage(message.Chat, "Сейчас нет команды, которую можно отменить");
            return;
        }
        await bot.SendMessage(message.Chat, "Команда отменена");
    }
}