using System.Text;
using TeleBotInfr.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

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

    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, answer);
    }
}