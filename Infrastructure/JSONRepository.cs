using Infrastructure;
using System.Text.Json;
using dotenv.net.Utilities;

namespace AICore;

public class JSONRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    private FileInfo fileInfo;
    
    public JSONRepository(IConfig config)
    {
        fileInfo = new FileInfo(config.UsersPath);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        var all = await GetAllDictionary();
        return all.Values;
    }

    public async Task<TEntity> GetById(TId id)
    {
        var entities = await GetAllDictionary();
        entities.TryGetValue(id, out var entity);
        return entity;
    }

    public async Task Add(TEntity entity)
    {
        var entities = await GetAllDictionary();
        if (entities.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with id: {entity.Id} already exists");
        entities.Add(entity.Id, entity);
        var json = JsonSerializer.Serialize(entities);
        await File.WriteAllTextAsync(fileInfo.Name, json);
    }

    public async Task Delete(TId id)
    {
        var entities = await GetAllDictionary();
        if (!entities.ContainsKey(id))
            throw new InvalidOperationException($"Entity with id: {id} does not exist");
        entities.Remove(id);
        var json = JsonSerializer.Serialize(entities);
        await File.WriteAllTextAsync(fileInfo.Name, json);

    }

    public async Task Update(TEntity entity)
    {
        var entities = await GetAllDictionary();
        if (!entities.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with id: {entity.Id} does not exist");
        entities[entity.Id] = entity;
        var json = JsonSerializer.Serialize(entities);
        await File.WriteAllTextAsync(fileInfo.Name, json);
    }

    private async Task<Dictionary<TId, TEntity>> GetAllDictionary()
    {
        var json = await File.ReadAllTextAsync(fileInfo.Name);
        var entities = JsonSerializer.Deserialize<Dictionary<TId,TEntity>>(json);
        return entities ?? new Dictionary<TId, TEntity>();
    }
}