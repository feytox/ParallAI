using AICore.Repositories;
using AICore.States;
using TeleBotInfr.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;


namespace TeleBot.Commands.Presets;

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