using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core;
using ParallAI.Core.States;
using ParallAI.Core.States.Common;
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

        services.AddScannedHandlers<ICommand>(assembly, typeof(CommandAttribute), typeof(MainMenuAttribute));
        services.AddScannedHandlers<ICallbackQuery>(assembly, typeof(CallbackQueryAttribute));
        
        services.RegisterSequentialState<PresetState, PresetStep>(assembly, true);
        services.RegisterSequentialState<RequestState, RequestStep>(assembly, true);
        services.AddScannedHandlers<ICommand, CommandAttribute>(assembly);
        services.AddScannedHandlers<ICallbackQuery, CallbackQueryAttribute>(assembly);
        
        services.RegisterSequentialState<RequestState, RequestStep>(assembly, true);
        services.AddSingleton<IStateAction, MainMenuStateAction>();
        
        services.AddSettingsState<PresetSettingsState, PresetSettingsHandler>();
    }

    private static void AddScannedHandlers<TInterface>(
        this IServiceCollection services,
        Assembly assembly,
        params Type[] attributeTypes)
    {
        var implementationTypes = assembly.GetTypes()
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .ToList();
        
        foreach (var type in implementationTypes)
            services.AddSingleton(typeof(TInterface), type);
        
        foreach (var attrType in attributeTypes)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(attrType);

            services.AddSingleton(enumerableType, _ =>
            {
                var attributes = implementationTypes
                    .SelectMany(t => t.GetCustomAttributes(attrType, false))
                    .ToList();

                var typedArray = Array.CreateInstance(attrType, attributes.Count);
                for (var i = 0; i < attributes.Count; i++)
                    typedArray.SetValue(attributes[i], i);

                return typedArray;
            });
        }
    }

    private static void RegisterSequentialState<TState, TStep>(this IServiceCollection services, Assembly assembly,
        bool endSilently)
        where TState : SequentialState<TStep>
        where TStep : notnull
    {
        services.AddSingleton<IStateAction, SequentialStateAction<TState, TStep>>(sp =>
            new SequentialStateAction<TState, TStep>(
                sp.GetRequiredService<IEnumerable<IStepStateAction<TState, TStep>>>(),
                endSilently
            ));

        var stepTypes = assembly.GetTypes()
            .Where(t => typeof(IStepStateAction<TState, TStep>).IsAssignableFrom(t)
                        && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in stepTypes)
            services.AddSingleton(typeof(IStepStateAction<TState, TStep>), type);
    }

    private static void AddSettingsState<TState, THandler>(this IServiceCollection services)
        where TState : SettingsState where THandler : SettingsHandler<TState>
    {
        services.AddSingleton<SettingsHandler<TState>, THandler>();
        services.AddSingleton<THandler>();
        services.AddSingleton<IStateAction, SettingsStateAction<TState>>();
    }
}