using System.Reflection;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

[Command("/help", "выводит список доступных команд")]
public class HelpCommand(Lazy<IEnumerable<ICommand>> commands) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var answer = new StringBuilder();
        answer.Append("Доступные команды:\n");
        foreach (var command in commands.Value)
        {
            var attr = command.GetType().GetCustomAttribute<CommandAttribute>();
            if (attr is not null)
                answer.Append($"{attr.Name} - {attr.Description}\n");
        }
        await bot.SendMessage(message.Chat, answer.ToString());
    }
}