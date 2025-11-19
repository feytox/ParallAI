using MongoDB.Driver;
using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;

namespace ParallAI.Infrastructure.Mongo;

public class MongoRepository<TEntity, TId>(IMongoDatabase database, string collectionName)
    : IRepository<TEntity, TId> where TEntity : IEntity<TId> where TId : notnull
{
    private readonly IMongoCollection<TEntity> collection = database.GetCollection<TEntity>(collectionName);

    public async Task<TEntity?> GetById(TId id)
    {
        return await collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task Add(TEntity entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public async Task Delete(TId id)
    {
        var result = await collection.DeleteOneAsync(e => e.Id.Equals(id));
        if (result.DeletedCount == 0)
            throw new InvalidOperationException($"{typeof(TEntity).Name} with id: {id} was not deleted)");
    }

    public async Task Update(TEntity entity)
    {
        var result = await collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
        if (result.MatchedCount == 0)
            throw new InvalidOperationException($"{typeof(TEntity).Name} with id: {entity.Id} was not updated");
    }
}