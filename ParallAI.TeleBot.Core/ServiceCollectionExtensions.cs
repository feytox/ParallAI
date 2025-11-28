using Microsoft.Extensions.DependencyInjection;
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
}