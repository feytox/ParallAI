using AICore;
using AICore.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure;

public static class MongoMappings
{
    public static void Setup()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
        
        BsonClassMap.RegisterClassMap<User>(classMap =>
        {
            classMap.MapCreator(e => new User(e.Id));
            classMap.AutoMap();
        });

        BsonClassMap.RegisterClassMap<AiModel>(classMap =>
        {
            classMap.MapCreator(m => new AiModel(m.Id, m.Name));
            classMap.AutoMap();
        });
    }
}