using Autofac;
using Infrastructure;

namespace TeleBot;

public static class Program
{
    public static void Main()
    {
        var container = CreateContainer();
        using var scope = container.BeginLifetimeScope();
        
        scope.Resolve<Bot>();
        Console.Write("Бот запущен. Для остановки нажмите ENTER...");
        Console.ReadLine();
    }
    
    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        builder.Register(_ => EnvConfig.Load()).As<IConfig>().SingleInstance();
        builder.RegisterType<Bot>().AsSelf().SingleInstance();
        return builder.Build();
    }
}