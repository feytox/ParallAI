namespace Infrastructure;

public interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<bool> TryGetById(TId id, out TEntity entity);
    Task<bool> TryAdd(TEntity entity);
    Task<bool> TryDelete(TId id);
    Task<bool> TryUpdate(TEntity entity);
}

public static class RepositoryExtensions
{
    public static async Task<TEntity> GetById<TEntity, TId>(this IRepository<TEntity,TId> repository, TId id) 
        where TEntity : IEntity<TId>
    {
        var hasValue = await repository.TryGetById(id, out var entity);
        if (hasValue)
            return entity;
        throw new InvalidOperationException($"{typeof(TEntity).Name} with {id} not found");
    }
    
    public static async Task Add<TEntity, TId>(this IRepository<TEntity,TId> repository, TEntity entity) 
        where TEntity : IEntity<TId>
    {
        var successAdd = await repository.TryAdd(entity);
        if (!successAdd)
            throw new InvalidOperationException($"{typeof(TEntity).Name} with {entity.Id} already exists");
    }

    public static async Task Delete<TEntity, TId>(this IRepository<TEntity, TId> repository, TId id)
        where TEntity : IEntity<TId>
    {
        var successDelete = await repository.TryDelete(id);
        if (!successDelete)
            throw new InvalidOperationException($"{typeof(TEntity).Name} with {id} not found");
    }

    public static async Task Update<TEntity, TId>(this IRepository<TEntity, TId> repository, TEntity entity)
        where TEntity : IEntity<TId>
    {
        var successUpdate = await repository.TryUpdate(entity);
        if (!successUpdate)
            throw new InvalidOperationException($"{typeof(TEntity).Name} with {entity.Id} is not found");
    }
}