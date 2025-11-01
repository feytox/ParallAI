namespace AICore.Entities;

public abstract class Entity<TId>(TId id) : IEntity<TId> where TId : notnull
{
    public TId Id { get; } = id;

    private bool Equals(Entity<TId> other)
    {
        
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) 
            return false;
        if (ReferenceEquals(this, obj)) 
            return true;
        return obj.GetType() == GetType() && Equals((Entity<TId>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }

    public override string ToString()
    {
        return $"{GetType().Name}({nameof(Id)}: {Id})";
    }
}