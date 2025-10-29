namespace Infrastructure.Entities;

// TODO: remove boilerplate (issue #25)
public class UserDto
{
    public long Id { get; private set; }
    public ICollection<Guid> Models { get; private set; }
}