using AICore.States;
using Infrastructure;

namespace AICore;

public class User(long id) : Entity<long>(id)
{
    public UserState? State { get; set; }
}