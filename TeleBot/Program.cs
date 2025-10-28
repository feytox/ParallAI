using System.Reflection;
using AICore;
using AICore.Repositories;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Infrastructure.Repositories;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User = AICore.Entities.User;

namespace TeleBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
            .ConfigureServices(ConfigureServices)
            .Build();

        await host.RunAsync();
    }

    private static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<Bot>().As<IHostedService>().SingleInstance();
        builder.Register(_ => EnvConfig.Load()).As<IConfig>().SingleInstance();
        builder.RegisterType<UserRepository>().As<IRepository<User, long>>();
        builder.RegisterType<AiModelRepository>().As<IRepository<AiModel, Guid>>();
        builder.RegisterType<AppDbContext>().AsSelf().InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(typeof(ICommand).Assembly).As<ICommand>().SingleInstance();
        builder.RegisterType<CommandHandler>().AsSelf().SingleInstance();
        builder.Register(c =>
            c.ComponentRegistry.Registrations
                .Select(r => r.Activator.LimitType)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<CommandAttribute>())
        ).As<IEnumerable<CommandAttribute>>().SingleInstance();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection builder)
    {
        // TODO: use real database
        builder.AddDbContextFactory<AppDbContext>(options => options
            .UseInMemoryDatabase("ParallAIDB")
        );

        var mapConfig = MappingConfigurator.ConfigureMappings();
        builder.AddSingleton(mapConfig);
        builder.AddScoped<IMapper, ServiceMapper>();
    }
}