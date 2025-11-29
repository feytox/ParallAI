using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;

namespace ParallAI.TeleBot.Core;

public static class ServiceCollectionExtensions
{
    public static void AddTelebotCore(this IServiceCollection services)
    {
        var coreAssembly = typeof(ICommand).Assembly;
        
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<CallbackQueryHandler>();
        services.AddSingleton<StateHandler>();
        
        var stateActionTypes = coreAssembly.GetTypes()
            .Where(t => typeof(IStateAction).IsAssignableFrom(t) 
                        && !t.IsInterface 
                        && !t.IsAbstract 
                        && !t.IsGenericTypeDefinition);
        foreach (var type in stateActionTypes)
            services.AddSingleton(typeof(IStateAction), type);
    }
    
    public static void RegisterSequentialState<TState, TStep>(this IServiceCollection services, Assembly assembly,
        bool endSilently) // мб отдельный класс создать 
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
}