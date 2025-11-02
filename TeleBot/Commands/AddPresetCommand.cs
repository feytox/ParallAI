#region

using AICore.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion


namespace TeleBot.Commands;

[Command("/addpresetwithcutesister", "добавить пресет команды с любимой аниме сестренкой(тестовая команда)")]
public class AddPresetCommand : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        user.StateMachine.Set(new PresetState());
        await bot.SendMessage(message.Chat, "Братик, только не сюда! Скажи хоть название своего нового пресета..");
    }
}