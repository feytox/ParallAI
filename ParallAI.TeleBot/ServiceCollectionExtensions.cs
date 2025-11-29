using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core;
using ParallAI.Core.States;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
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
}