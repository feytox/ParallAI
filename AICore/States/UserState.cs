using System.Collections.Immutable;
using MongoDB.Bson.Serialization.Attributes;

namespace AICore.States;

public abstract class UserState
{
    [BsonElement("Index")]
    private int index = 0;

    [BsonIgnore]
    public UserStateType CurrentStep => index < steps.Length ? steps[index] : default;
    
    private bool isCompleted => index >= steps.Length - 1;
    
    public bool Next()
    {
        if (isCompleted)
            return false;
        index++;
        return true;
    }
    
    protected abstract ImmutableArray<UserStateType> steps { get; }
}