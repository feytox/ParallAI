namespace Infrastructure.Entities;

public class UserDto
{
    public long Id { get; private set; }
    public ICollection<Guid> Models { get; private set; }
}