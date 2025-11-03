using System.Collections.Immutable;
using MongoDB.Bson.Serialization.Attributes;

namespace AICore.States;

public abstract class UserState
{
    public abstract bool IsCompleted { get; }
}
