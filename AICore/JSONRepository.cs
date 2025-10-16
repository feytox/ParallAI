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
        await using var stream = File.OpenRead(filePath);
        var entities = await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream);
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
        await using var stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, entities.Append(entity));
    }

    public async Task Delete(TId id)
    {
        var entities = await GetAll();
        await using var stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, entities.Where(e => !e.Id.Equals(id)));

    }

    public async Task Update(TEntity entity)
    {
        var entities = await GetAll();
        await using var stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, entities.Select(e => e.Id.Equals(entity.Id) ? e : entity));
    }
}