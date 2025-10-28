namespace AICore.Entities;

public interface IEntity<TId>
{
    public TId Id { get; }
}