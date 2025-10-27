using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBot;

public abstract class SingleCommand : ICommand
{
    async Task<NextCommandInfo?> ICommand.Execute(Message message, ITelegramBotClient bot)
    {
        await Execute(message, bot);
        return null;
    }

    protected abstract Task Execute(Message message, ITelegramBotClient bot);
}