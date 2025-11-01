#region

using AICore.Entities;
using AICore.ValueTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

#endregion

namespace Infrastructure.Mongo;

public static class MongoMappings
{
    public static void Setup()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        
        BsonClassMap.RegisterClassMap<User>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapProperty("models");
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
    }
}