using ParallAI.Core.Entities;

namespace ParallAI.Core.Services;

public interface IMetricService
{
    public void SaveGeneration(Guid requestId, User user);

    public void SaveComparison(Guid requestId, User user);
}