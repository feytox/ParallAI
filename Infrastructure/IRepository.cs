namespace Infrastructure;

interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(TId id);
    void Add(TEntity entity);
    Task Delete(TId id);
    Task Save();
}