using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.Services;

namespace ParallAI.Metrics;


public class MetricService(IRequestMetricRepository repository) : IMetricService
{

    public void SaveGeneration(Guid requestId, User user) => SaveRequest(requestId, user, RequestType.Single);

    public void SaveComparison(Guid requestId, User user) => SaveRequest(requestId, user, RequestType.Comparison);

    private void SaveRequest(Guid requestId, User user, RequestType requestType)
    {
        var metric = new RequestMetric(
            requestId,
            user.Id,
            requestType.ToString(),
            DateTime.UtcNow
        );
        _ = repository.Add(metric);
    }
}