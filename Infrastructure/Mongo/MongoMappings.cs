#region

using System.Reflection;
using AICore.Entities;
using AICore.States;
using AICore.ValueTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

#endregion

namespace Infrastructure.Mongo;

public static class MongoMappings
{
    // TODO: refactor
    public static void Setup()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        
        BsonClassMap.RegisterClassMap<User>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapProperty("Models");
        });

        BsonClassMap.RegisterClassMap<AiModel>(classMap =>
        {
            classMap.AutoMap();
        });

        BsonClassMap.RegisterClassMap<AiProvider>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIsRootClass(true);
        });
        
        BsonClassMap.RegisterClassMap<OpenAICompatibleProvider>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetDiscriminator("openai_compatible");
        });
        
        BsonClassMap.RegisterClassMap<GeminiProvider>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetDiscriminator("gemini");
        });
        
        BsonClassMap.RegisterClassMap<OpenRouterProvider>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetDiscriminator("openrouter");
        });
        
        BsonClassMap.RegisterClassMap<UserStateMachine>(classMap =>
        {
            classMap.AutoMap();
        });

        BsonClassMap.RegisterClassMap<UserState>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIsRootClass(true); 
        });

        var types = Assembly.GetAssembly(typeof(UserState))!
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(UserState).IsAssignableFrom(t));
        foreach (var type in types)
            BsonClassMap.LookupClassMap(type);
    }
}