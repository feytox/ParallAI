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

        services.AddScannedHandlers<ICommand>(assembly, typeof(CommandAttribute), typeof(MainMenuAttribute));
        services.AddScannedHandlers<ICallbackQuery>(assembly, typeof(CallbackQueryAttribute));
        
        services.RegisterSequentialState<RequestState, RequestStep>(assembly, true);
        services.AddSingleton<IStateAction, MainMenuStateAction>();
        
        services.AddSettingsState<PresetSettingsState, PresetSettingsHandler>();
    }
    
    private static void AddSettingsState<TState, THandler>(this IServiceCollection services)
        where TState : SettingsState where THandler : SettingsHandler<TState>
    {
        services.AddSingleton<SettingsHandler<TState>, THandler>();
        services.AddSingleton<THandler>();
        services.AddSingleton<IStateAction, SettingsStateAction<TState>>();
    }
}