using Infrastructure;

namespace AICore;

public class User(long id) : Entity<long>(id)
{
    public List<Preset> Presets { get; set; } = new();
}