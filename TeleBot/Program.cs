using ParallAI.Infrastructure;

namespace TeleBot;

public static class Program
{
    public static void Main()
    {
        using var bot = new Bot(EnvConfig.Instance.BotToken);
        Console.Write("Бот запущен. Для остановки нажмите ENTER...");
        Console.ReadLine();
    }
}