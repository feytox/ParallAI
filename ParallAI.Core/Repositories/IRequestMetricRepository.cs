using ParallAI.Core.Entities;

namespace ParallAI.Core.Repositories;

public interface IRequestMetricRepository
{
    Task Add(RequestMetric metric);
}
