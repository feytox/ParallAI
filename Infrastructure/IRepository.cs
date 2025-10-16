namespace Infrastructure;

public interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(TId id);
    Task Add(TEntity entity);
    Task Delete(TId id);
    Task Update(TEntity entity);
}