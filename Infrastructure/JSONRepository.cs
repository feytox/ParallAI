using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public class JSONRepository<TEntity, TId> : IRepository<TEntity, TId>, IHostedService where TEntity : IEntity<TId> where TId : notnull
{
    private readonly FileInfo fileInfo;
    private Dictionary<TId, TEntity> entities;
    
    public JSONRepository(string filePath)
    {
        fileInfo = new FileInfo(filePath);
    }

    public Task<IEnumerable<TEntity>> GetAll()
    {
        return Task.FromResult<IEnumerable<TEntity>>(entities.Values);
    }

    public Task<TEntity?> GetById(TId id)
    {
        entities.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task Add(TEntity entity)
    {
        if (!entities.TryAdd(entity.Id, entity))
            throw new InvalidOperationException($"Entity with id: {entity.Id} already exists");
        return Task.CompletedTask;
    }

    public Task Delete(TId id)
    {
        if (!entities.Remove(id))
            throw new InvalidOperationException($"Entity with id: {id} does not exist");
        return Task.CompletedTask;
    }

    public Task Update(TEntity entity)
    {
        if (!entities.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with id: {entity.Id} does not exist");
        entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!fileInfo.Exists)
        {
            entities = new Dictionary<TId, TEntity>();
            return;
        }
        
        var json = await File.ReadAllTextAsync(fileInfo.Name, cancellationToken);
        entities = JsonSerializer.Deserialize<Dictionary<TId,TEntity>>(json) 
                   ?? new Dictionary<TId, TEntity>();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(entities);
        await File.WriteAllTextAsync(fileInfo.Name, json, cancellationToken);
    }
}