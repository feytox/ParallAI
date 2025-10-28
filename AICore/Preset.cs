using Infrastructure;

namespace AICore;

// фиктивный пресет
public class Preset(long id, string name) : Entity<long>(id)
{
    public string Name { get; set; } = name;
}
