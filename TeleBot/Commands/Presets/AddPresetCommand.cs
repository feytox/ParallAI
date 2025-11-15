using AICore.Repositories;
using TeleBot.Commands.Common;
using TeleBot.Example.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;


namespace TeleBot.Example.Commands;

[Command("/addpreset", "добавить пресет")]
public class AddPresetCommand(IRepository<User, long> users) 
    : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        user.StateMachine.Set(new PresetState());
        await bot.SendMessage(message.Chat, "Введите название нового пресета");
    }
}