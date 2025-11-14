using AICore.Entities;

namespace AICore.Repositories;

public interface IRepository<TEntity, in TId> where TEntity : IEntity<TId> where TId : notnull
{
    Task<TEntity?> GetById(TId id);
    Task Add(TEntity entity);
    Task Delete(TId id);
    Task Update(TEntity entity);
}