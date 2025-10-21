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

    public Task<bool> TryGetById(TId id, out TEntity entity)
    {
        var result = entities.TryGetValue(id, out var value);
        entity = value;
        return Task.FromResult(result);
    }

    public Task<bool> TryAdd(TEntity entity)
    {
        return Task.FromResult(entities.TryAdd(entity.Id, entity));
    }

    public Task<bool> TryDelete(TId id)
    {
        return Task.FromResult(entities.Remove(id));
    }

    public Task<bool> TryUpdate(TEntity entity)
    {
        if (!entities.ContainsKey(entity.Id))
            return Task.FromResult(false);
        entities[entity.Id] = entity;
        return Task.FromResult(true);
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