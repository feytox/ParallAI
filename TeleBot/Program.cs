using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using AICore;
using Microsoft.Extensions.Hosting;

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
        builder.RegisterType<JSONRepository<User, int>>().As<IRepository<User, int>>().SingleInstance();
    }
}