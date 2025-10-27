using System.Text;
using TeleBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

[Command("/help", "вывод списка доступных команд")]
public class HelpCommand : SingleCommand
{
    private readonly string _answer;

    public HelpCommand(IEnumerable<CommandAttribute> attributes)
    {
        var answer = new StringBuilder();
        answer.Append("Доступные команды:");
        foreach (var attribute in attributes
                     .Where(a => a.Type == CommandType.Single || a.Type == CommandType.FirstStep))
        {
            answer.Append($"\n{attribute.Name} - {attribute.Description}");
        }
        _answer =  answer.ToString();
    }

    protected override async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, _answer);
    }
}