using Infrastructure;
using System.Text.Json;

namespace AICore;

public class JSONRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    private string filePath;
    
    public JSONRepository(string filepath)
    {
        this.filePath = filepath;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        var json = await File.ReadAllTextAsync(filePath);
        var entities = JsonSerializer.Deserialize<IEnumerable<TEntity>>(json);
        return entities ?? Enumerable.Empty<TEntity>();
    }

    public async Task<TEntity> GetById(TId id)
    {
        var entities = await GetAll();
        return entities.FirstOrDefault(e => e.Id.Equals(id));
    }

    public async Task Add(TEntity entity)
    {
        var entities = await GetAll();
        var json = JsonSerializer.Serialize(entities.Append(entity));
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task Delete(TId id)
    {
        var entities = await GetAll();
        var json = JsonSerializer.Serialize(entities.Where(e => !e.Id.Equals(id)));
        await File.WriteAllTextAsync(filePath, json);

    }

    public async Task Update(TEntity entity)
    {
        var entities = await GetAll();
        var json = JsonSerializer.Serialize(entities.Select(e => !e.Id.Equals(entity.Id) ? e : entity));
        await File.WriteAllTextAsync(filePath, json);
    }
}