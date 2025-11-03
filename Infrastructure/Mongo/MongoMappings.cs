#region

using System.Reflection;
using AICore.Entities;
using AICore.States;
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
            classMap.MapCreator(e => new User(e.Id));
            classMap.AutoMap();
        });

        BsonClassMap.RegisterClassMap<AiModel>(classMap =>
        {
            classMap.MapCreator(m => new AiModel(m.Id, m.Name));
            classMap.AutoMap();
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