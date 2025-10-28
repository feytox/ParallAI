using AICore.Entities;

namespace AICore;

public class Preset(Guid id, string name) : Entity<Guid>(id)
{
    public string Name { get; set; } = name;
}
