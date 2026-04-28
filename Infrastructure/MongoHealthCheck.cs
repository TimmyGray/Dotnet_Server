using BuyingLibrary.AppSettings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Aspnet_server.Infrastructure;

public sealed class MongoHealthCheck : IHealthCheck
{
    private readonly DataBaseOptions _options;

    public MongoHealthCheck(IOptions<DataBaseOptions> options)
    {
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new MongoClient(_options.DataBaseConnection);
            var database = client.GetDatabase(_options.DataBase);
            await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy("MongoDB is reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("MongoDB check failed", ex);
        }
    }
}
