using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core;
using ParallAI.Core.States;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.Settings;
using ParallAI.TeleBot.StateActions;

namespace ParallAI.TeleBot;

public static class ServiceCollectionExtensions
{
    public static void AddTeleBot(this IServiceCollection services)
    {
        var assembly = typeof(Bot).Assembly;

        services.AddSingleton<Bot>()
            .AddHostedService(sp => sp.GetRequiredService<Bot>());

        services.AddTransient<IFileService, TgFileService>();
        services.AddSingleton<MediaGroupCollector>();

        services.AddScanned<ICommand>(assembly);
        services.AddAttribute<ICommand, CommandAttribute>();
        services.AddAttribute<ICommand, MainMenuAttribute>();
        
        services.AddScanned<ICallbackQuery>(assembly);
        services.AddAttribute<ICallbackQuery, CallbackQueryAttribute>();

        services.RegisterSequentialState<RequestState, RequestStep>(assembly, true);
        services.AddSingleton<IStateAction, MainMenuStateAction>();

        services.AddSettingsState<PresetSettingsState, PresetSettingsHandler>();
    }

    private static void AddScanned<TInterface>(this IServiceCollection services, Assembly assembly)
        where TInterface : notnull
    {
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<TInterface>())
                .As<TInterface>()
                .WithSingletonLifetime()
        );
    }

    private static void AddAttribute<TInterface, TAttribute>(this IServiceCollection services)
        where TAttribute : Attribute
        where TInterface : notnull
    {
        services.AddSingleton<IEnumerable<(TInterface services, TAttribute attribute)>>(provider => provider
            .GetServices<TInterface>()
            .SelectMany(service => service.GetType().GetCustomAttributes<TAttribute>()
                .Select(attribute => (service, attribute)))
        );
    }

    private static void AddSettingsState<TState, THandler>(this IServiceCollection services)
        where TState : SettingsState
        where THandler : SettingsHandler<TState>
    {
        services.AddSingleton<SettingsHandler<TState>, THandler>();
        services.AddSingleton<THandler>();
        services.AddSingleton<IStateAction, SettingsStateAction<TState>>();
    }
}