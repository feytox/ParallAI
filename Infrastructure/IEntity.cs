namespace Infrastructure;

public interface IEntity<TId>
{
    public TId Id { get; }
}