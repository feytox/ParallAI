using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.Repositories;
using ParallAI.Core.Services;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Config;
using ParallAI.Infrastructure.Mongo;
using ParallAI.Infrastructure.Services;

namespace ParallAI.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        MongoMappings.Setup();
        services.AddSingleton<IConfig>(EnvConfig.Load());
        
        services.AddHttpClient();
        
        services.AddSingleton<IMongoClient>(sp => GetMongoClient(sp.GetRequiredService<IConfig>()));
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase("ParallAIDB"));
        
        services.AddSingleton<IRepository<User, long>>(sp =>
            new MongoRepository<User, long>(
                sp.GetRequiredService<IMongoDatabase>(),
                sp.GetRequiredService<IConfig>().UsersCollection
            ));
        
        RegisterProvider<GeminiGenHandler, GeminiProvider>(services);
        RegisterProvider<OpenAiGenHandler, OpenAICompatibleProvider>(services);
        RegisterProvider<OpenRouterGenHandler, OpenRouterProvider>(services);

        return services;
    }
    
    private static MongoClient GetMongoClient(IConfig config)
    {
        var settings = MongoClientSettings.FromConnectionString(config.MongoConnectionString);
        settings.SocketTimeout = TimeSpan.FromSeconds(5);
        settings.ConnectTimeout = TimeSpan.FromSeconds(5);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        return new MongoClient(settings);
    }

    private static void RegisterProvider<THandler, TProvider>(IServiceCollection services)
        where TProvider : AiProvider
        where THandler : class, IGenerationHandler
    {
        services.AddTransient<THandler>();
        services.AddTransient<IGenerationHandler>(sp => sp.GetRequiredService<THandler>());

        services.AddSingleton<IGenService>(sp =>
        {
            THandler HandlerFactory(TProvider provider, AiModel model)
                => ActivatorUtilities.CreateInstance<THandler>(sp, provider, model);

            return new HandledGenService<THandler, TProvider>(HandlerFactory);
        });
    }
}