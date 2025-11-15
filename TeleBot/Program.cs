#region

using System.Reflection;
using AICore.Repositories;
using AICore.Services;
using AICore.States;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Config;
using Infrastructure.Mongo;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using TeleBot.Commands.Common;
using TeleBot.StateActions.Common;
using User = AICore.Entities.User;

#endregion

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
        builder.Register(c => GetMongoClient(c.Resolve<IConfig>()))
            .As<IMongoClient>().SingleInstance();
        builder.Register(c => c.Resolve<IMongoClient>().GetDatabase("ParallAIDB"))
            .As<IMongoDatabase>().SingleInstance();
        builder.Register(c =>
                new MongoRepository<User, long>(c.Resolve<IMongoDatabase>(), c.Resolve<IConfig>().UsersCollection))
            .As<IRepository<User, long>>().SingleInstance();

        builder.RegisterType<GeminiGenService>().As<IGenService>().SingleInstance();
        builder.RegisterType<OpenAiGenService>().As<IGenService>().SingleInstance();
        builder.RegisterType<OpenRouterGenService>().As<IGenService>().SingleInstance();
        builder.RegisterType<GenerationService>().AsSelf().SingleInstance();
        
        builder.RegisterAssemblyTypes(typeof(ICommand).Assembly).As<ICommand>().SingleInstance();
        builder.RegisterType<CommandHandler>().AsSelf().SingleInstance();
        builder.Register(c =>
            c.ComponentRegistry.Registrations
                .Select(r => r.Activator.LimitType)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<CommandAttribute>())
        ).As<IEnumerable<CommandAttribute>>().SingleInstance();

        builder.RegisterAssemblyTypes(typeof(IStateAction).Assembly).As<IStateAction>().SingleInstance();
        builder.RegisterType<StateHandler>().AsSelf().SingleInstance();
        
        RegisterSequentialState<PresetState, PresetStep>(builder);
    }

    private static void RegisterSequentialState<TState, TStep>(ContainerBuilder builder)
        where TState : SequentialState<TStep> where TStep : notnull
    {
        builder.RegisterType<SequentialStateAction<TState, TStep>>()
            .As<IStateAction>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(typeof(IStepStateAction<TState, TStep>).Assembly)
            .As<IStepStateAction<TState, TStep>>()
            .SingleInstance();
    }

    private static MongoClient GetMongoClient(IConfig config)
    {
        var settings = MongoClientSettings.FromConnectionString(config.MongoConnectionString);
        settings.SocketTimeout = TimeSpan.FromSeconds(5);
        settings.ConnectTimeout = TimeSpan.FromSeconds(5);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        return new MongoClient(settings);
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection builder)
    {
        MongoMappings.Setup();
        builder.AddHttpClient();
    }
}