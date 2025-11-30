using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;

namespace ParallAI.TeleBot.Core;

public static class ServiceCollectionExtensions
{
    public static void AddTelebotCore(this IServiceCollection services)
    {
        var assembly = typeof(ICommand).Assembly;
        
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<CallbackQueryHandler>();
        services.AddSingleton<StateHandler>();
        
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<IStateAction>()
                .Where(c => c is { IsInterface: false, IsAbstract: false, IsGenericTypeDefinition: false }))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );
    }
    
    public static void RegisterSequentialState<TState, TStep>(this IServiceCollection services, Assembly assembly,
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
}