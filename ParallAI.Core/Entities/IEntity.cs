namespace ParallAI.Core.Entities;

public interface IEntity<out TId> where TId : notnull
{
    public TId Id { get; }
}