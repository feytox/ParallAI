#region

using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

#endregion

namespace TeleBot.Commands;

[Command("/help", "вывод списка доступных команд")]
public class HelpCommand : ICommand
{
    private readonly string answer;

    public HelpCommand(IEnumerable<CommandAttribute> attributes)
    {
        var result = new StringBuilder();
        result.Append("Доступные команды:");
        foreach (var attribute in attributes)
        {
            result.Append($"\n{attribute.Name} - {attribute.Description}");
        }

        answer = result.ToString();
    }

    public async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(message.Chat, answer);
    }
}