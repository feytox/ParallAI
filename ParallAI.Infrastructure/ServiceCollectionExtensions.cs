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
    public static void AddInfrastructure(this IServiceCollection services)
    {
        MongoMappings.Setup();
        services.AddSingleton<IConfig>(EnvConfig.Load());
        services.AddHttpClient<HttpClient>(client => client.Timeout = TimeSpan.FromMinutes(5));
        
        services.AddSingleton<IMongoClient>(sp => GetMongoClient(sp.GetRequiredService<IConfig>()));
        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase("ParallAIDB"));
        
        services.AddSingleton<IRepository<User, long>>(sp =>
            new MongoRepository<User, long>(
                sp.GetRequiredService<IMongoDatabase>(),
                sp.GetRequiredService<IConfig>().UsersCollection
            ));
        
        services.AddSingleton<CancelTokenSourceStorage>();
        
        services.AddProvider<GeminiGenHandler, GeminiProvider>();
        services.AddProvider<OpenAiGenHandler, OpenAICompatibleProvider>();
        services.AddProvider<OpenRouterGenHandler, OpenRouterProvider>();
    }
    
    private static MongoClient GetMongoClient(IConfig config)
    {
        var settings = MongoClientSettings.FromConnectionString(config.MongoConnectionString);
        settings.SocketTimeout = TimeSpan.FromSeconds(5);
        settings.ConnectTimeout = TimeSpan.FromSeconds(5);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        return new MongoClient(settings);
    }

    private static void AddProvider<THandler, TProvider>(this IServiceCollection services)
        where TProvider : AiProvider
        where THandler : class, IGenerationHandler
    {
        services.AddTransient<THandler>();
        services.AddTransient<IGenerationHandler>(sp => sp.GetRequiredService<THandler>());
        
        services.AddSingleton<Func<TProvider, AiModel, THandler>>(sp =>
            (provider, model) => ActivatorUtilities.CreateInstance<THandler>(sp, provider, model));

        services.AddSingleton<IGenService, HandledGenService<THandler, TProvider>>();
    }
}