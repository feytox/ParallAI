using MongoDB.Driver;
using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;

namespace ParallAI.Infrastructure.Mongo;

public class RequestMetricRepository(IMongoDatabase database) : IRequestMetricRepository
{
    private readonly IMongoCollection<RequestMetric> collection = database.GetCollection<RequestMetric>("RequestMetrics");

    public async Task Add(RequestMetric metric) => await collection.InsertOneAsync(metric);
}
