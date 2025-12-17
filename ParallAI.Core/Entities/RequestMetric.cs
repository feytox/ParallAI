namespace ParallAI.Core.Entities;

public class RequestMetric(Guid requestId, long userId, string requestType, DateTime requestTime) : Entity<Guid>(requestId)
{
    public long UserId { get; private set; } = userId;
    public string RequestType { get; private set; } = requestType;
    public DateTime RequestTime { get; private set; } = requestTime;
}
