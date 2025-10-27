using System.Reflection;
using AICore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.Hosting;
using TeleBot.Commands;
using TeleBot.Services;

namespace TeleBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
            .Build();

        await host.RunAsync();
    }

    private static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.Register(_ => EnvConfig.Load()).As<IConfig>().SingleInstance();
        builder.RegisterType<Bot>().As<IHostedService>().SingleInstance();
        builder.RegisterType<JSONRepository<User, long>>()
            .As<IRepository<User, long>>()
            .As<IHostedService>()
            .WithParameter(
                (info, _) => info.ParameterType == typeof(string),
                (_, ctx) => ctx.Resolve<IConfig>().UsersPath)
            .SingleInstance();
        builder.RegisterType<UserSessionService>().As<IUserSessionService>().SingleInstance();
        builder.RegisterAssemblyTypes(typeof(ICommand).Assembly).As<ICommand>().SingleInstance();
        builder.RegisterType<CommandHandler>().AsSelf().SingleInstance();
        builder.Register(c =>
            c.ComponentRegistry.Registrations
                .Select(r => r.Activator.LimitType)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<CommandAttribute>())
        ).As<IEnumerable<CommandAttribute>>().SingleInstance();
    }
}