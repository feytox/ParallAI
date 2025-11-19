using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Config;
using ParallAI.Infrastructure.Mongo;
using ParallAI.Infrastructure.Services;
using ParallAI.TeleBot;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using TeleBot.Example.States;

namespace ParallAI.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
            .ConfigureServices(ConfigureServices)
            .Build();

        await host.RunAsync();
    }

    private static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<Bot>().AsSelf().As<IHostedService>().SingleInstance();
        builder.Register(_ => EnvConfig.Load()).As<IConfig>().SingleInstance();
        builder.Register(c => GetMongoClient(c.Resolve<IConfig>()))
            .As<IMongoClient>().SingleInstance();
        builder.Register(c => c.Resolve<IMongoClient>().GetDatabase("ParallAIDB"))
            .As<IMongoDatabase>().SingleInstance();
        builder.Register(c =>
                new MongoRepository<User, long>(c.Resolve<IMongoDatabase>(), c.Resolve<IConfig>().UsersCollection))
            .As<IRepository<User, long>>().SingleInstance();

        builder.RegisterType<GenerationService>().AsSelf().SingleInstance();
        RegisterProvider<GeminiGenHandler, GeminiProvider>(builder);
        RegisterProvider<OpenAiGenHandler, OpenAICompatibleProvider>(builder);
        RegisterProvider<OpenRouterGenHandler, OpenRouterProvider>(builder);

        var teleBotAssembly = typeof(Bot).Assembly;
        var infrTeleBotAssembly = typeof(ICommand).Assembly;
        
        builder.RegisterType<TgFileService>().As<IFileService>();
        
        builder.RegisterAssemblyTypes(teleBotAssembly).As<ICommand>().SingleInstance();
        builder.RegisterType<CommandHandler>().AsSelf().SingleInstance();
        builder.Register(c =>
            c.ComponentRegistry.Registrations
                .Select(r => r.Activator.LimitType)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<CommandAttribute>())
        ).As<IEnumerable<CommandAttribute>>().SingleInstance();
        
        builder.RegisterAssemblyTypes(teleBotAssembly).As<ICallbackQuery>().SingleInstance();
        builder.RegisterType<CallbackQueryHandler>().AsSelf().SingleInstance();
        builder.Register(c =>
            c.ComponentRegistry.Registrations
                .Select(r => r.Activator.LimitType)
                .Where(t => typeof(ICallbackQuery).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<CallbackQueryAttribute>())
        ).As<IEnumerable<CallbackQueryAttribute>>().SingleInstance();

        builder.RegisterAssemblyTypes(infrTeleBotAssembly).As<IStateAction>().SingleInstance();
        builder.RegisterType<StateHandler>().AsSelf().SingleInstance();

        RegisterSequentialState<PresetState, PresetStep>(builder, teleBotAssembly);
        RegisterSequentialState<RequestState, RequestStep>(builder, teleBotAssembly);

        builder.RegisterType<MediaGroupCollector>().AsSelf().SingleInstance();
    }

    private static void RegisterProvider<THandler, TProvider>(ContainerBuilder builder)
        where TProvider : AiProvider
        where THandler : IGenerationHandler
    {
        builder
            .Register<IGenService>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return new HandledGenService<THandler, TProvider>(HandlerFactory);

                THandler HandlerFactory(TProvider provider, AiModel model)
                {
                    return context.Resolve<THandler>(
                        new TypedParameter(typeof(TProvider), provider),
                        new TypedParameter(typeof(AiModel), model));
                }
            })
            .SingleInstance();

        builder.RegisterType<THandler>().AsSelf().As<IGenerationHandler>();
    }

    private static void RegisterSequentialState<TState, TStep>(ContainerBuilder builder,  Assembly stepsAssembly)
        where TState : SequentialState<TStep> where TStep : notnull
    {
        builder.RegisterType<SequentialStateAction<TState, TStep>>()
            .As<IStateAction>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(stepsAssembly)
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