using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot.Commands;

[Command("/adduser", "Добавить пользователя")]
public class AddUserStartCommand : ICommand
{
    public async Task<NextCommandInfo?> Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Введите имя пользователя:");
        return new NextCommandInfo { NextCommandName = "//addusername" };
    }
}

[Command("//addusername", "Ввод имени", CommandType.Step)]
public class AddUserNameCommand : ICommand
{
    public async Task<NextCommandInfo?> Execute(Message message, ITelegramBotClient bot)
    {
        var userName = message.Text;

        if (userName.Length < 5)
        {
            await bot.SendMessage(message.Chat, "Имя слишком короткое. Попробуйте еще раз:");
            return new NextCommandInfo { NextCommandName = "//addusername" };
        }

        await bot.SendMessage(message.Chat, $"Отлично, пользователь {userName} добавлен!");
        return null;
    }
}