using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Commands.Common;
using ParallAI.TeleBot.Core.StateActions;

namespace ParallAI.TeleBot.Core;

public static class ServiceCollectionExtensions
{
    public static void AddTelebotCore(this IServiceCollection services)
    {
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<CallbackQueryHandler>();
        services.AddSingleton<StateHandler>();

        services.Scan(scan => scan
            .FromAssemblyOf<IStateAction>()
            .AddClasses(classes => classes.AssignableTo<IStateAction>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );
    }

    public static void AddSequentialState<TState, TStep>(this IServiceCollection services, Assembly assembly,
        bool endSilently)
        where TState : SequentialState<TStep>
        where TStep : notnull
    {
        services.AddSingleton<IStateAction, SequentialStateAction<TState, TStep>>(sp =>
            new SequentialStateAction<TState, TStep>(
                sp.GetRequiredService<IEnumerable<IStepStateAction<TState, TStep>>>(),
                endSilently
            ));

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<IStepStateAction<TState, TStep>>())
            .As<IStepStateAction<TState, TStep>>()
            .WithSingletonLifetime()
        );
    }
}