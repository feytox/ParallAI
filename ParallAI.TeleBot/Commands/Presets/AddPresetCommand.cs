using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Commands.Presets;

[Command("/addpreset", "добавить пресет")]
public class AddPresetCommand(IRepository<User, long> users) 
    : UserCommand(users)
{
    protected override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        user.StateMachine.Push(new PresetState());
        await bot.SendMessage(message.Chat, "Введите название нового пресета");
    }
}