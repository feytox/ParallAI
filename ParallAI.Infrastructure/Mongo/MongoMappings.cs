using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.Mongo;

public static class MongoMappings
{
    public static void Setup()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));

        BsonClassMap.RegisterClassMap<User>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapProperty("Models");
            classMap.MapProperty("Presets");
        });

        BsonClassMap.RegisterClassMap<UserStateMachine>(cm =>
        {
            cm.AutoMap();
            cm.MapProperty("States");
        });

        RegisterAutoMaps(typeof(AiModel), typeof(Preset));

        RegisterProviders();
        RegisterStates();
    }

    private static void RegisterProviders()
    {
        BsonClassMap.RegisterClassMap<AiProvider>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });

        RegisterDiscriminator<OpenAICompatibleProvider>("openai_compatible");
        RegisterDiscriminator<GeminiProvider>("gemini");
        RegisterDiscriminator<OpenRouterProvider>("openrouter");
    }

    private static void RegisterStates()
    {
        BsonClassMap.RegisterClassMap<UserState>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });

        var stateTypes = typeof(UserState).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(UserState).IsAssignableFrom(t));
        foreach (var type in stateTypes)
            BsonClassMap.LookupClassMap(type);
    }

    private static void RegisterAutoMaps(params Type[] types)
    {
        foreach (var type in types)
        {
            if (BsonClassMap.IsClassMapRegistered(type))
                continue;
            var cm = new BsonClassMap(type);
            cm.AutoMap();
            BsonClassMap.RegisterClassMap(cm);
        }
    }

    private static void RegisterDiscriminator<T>(string discriminator)
    {
        BsonClassMap.RegisterClassMap<T>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator(discriminator);
        });
    }
}