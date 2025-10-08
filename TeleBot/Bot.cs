using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBot;

public class Bot : IDisposable
{
    private readonly TelegramBotClient bot;
    private readonly CancellationTokenSource cts;

    public Bot(string token)
    {
        cts = new CancellationTokenSource();
        bot = new TelegramBotClient(token, cancellationToken: cts.Token);
        Subscribe();
    }

    private void Subscribe()
    {
        bot.OnMessage += OnMessage;
    }
    
    private async Task OnMessage(Message message, UpdateType type)
    {
        var reply = new ReplyParameters { MessageId = message.MessageId };
        await bot.SendMessage(message.Chat, "Йоооу, у тебя получилось!", replyParameters: reply);
    }

    ~Bot()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) 
            return;
        
        cts.Cancel();
        cts.Dispose();
    }
}