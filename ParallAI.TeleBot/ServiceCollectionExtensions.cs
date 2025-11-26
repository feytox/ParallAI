using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;

namespace ParallAI.TeleBot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTeleBot(this IServiceCollection services)
    {
        var assembly = typeof(Bot).Assembly;
        
        services.AddSingleton<Bot>()
                .AddHostedService(sp => sp.GetRequiredService<Bot>());

        services.AddTransient<IFileService, TgFileService>();
        services.AddSingleton<MediaGroupCollector>();
        
        AddScannedHandlers<ICommand, CommandAttribute>(services, assembly);
        AddScannedHandlers<ICallbackQuery, CallbackQueryAttribute>(services, assembly);
        
        RegisterSequentialState<PresetState, PresetStep>(services, assembly);
        RegisterSequentialState<RequestState, RequestStep>(services, assembly);
        
        return services;
    }
    
    private static void AddScannedHandlers<TInterface, TAttribute>(IServiceCollection services, Assembly assembly)
        where TAttribute : Attribute
    {
        var types = assembly.GetTypes()
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();
        
        foreach (var type in types)
            services.AddSingleton(typeof(TInterface), type);
        
        services.AddSingleton<IEnumerable<TAttribute>>(_ => 
            types.SelectMany(t => t.GetCustomAttributes<TAttribute>()));
    }
    
    private static void RegisterSequentialState<TState, TStep>(IServiceCollection services, Assembly assembly)
        where TState : SequentialState<TStep> 
        where TStep : notnull
    {
        services.AddSingleton<IStateAction, SequentialStateAction<TState, TStep>>();
        
        var stepTypes = assembly.GetTypes()
            .Where(t => typeof(IStepStateAction<TState, TStep>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            
        foreach (var type in stepTypes)
            services.AddSingleton(typeof(IStepStateAction<TState, TStep>), type);
    }
}