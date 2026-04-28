using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class ConnectorService : IService<Connector>
{
    private readonly IMongoCollection<Connector> _collection;
    private readonly ILogger<ConnectorService> _logger;

    public ConnectorService(MongoContext context, ILogger<ConnectorService> logger)
    {
        _collection = context.ConnectorsCollection;
        _logger = logger;
    }

    public async Task<List<Connector>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all connectors.");
        return await _collection.Find(FilterDefinition<Connector>.Empty).ToListAsync(cancellationToken);
    }

    public async Task<Connector?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching connector with id {Id}.", id);
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Connector> PostAsync(Connector connector, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting new connector: {Name}.", connector.Name);
        await _collection.InsertOneAsync(connector, cancellationToken: cancellationToken);
        return connector;
    }

    public async Task<Connector?> PutAsync(Connector updatedConnector, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Replacing connector with id {Id}.", updatedConnector.Id);
        var filter = Builders<Connector>.Filter.Eq(c => c.Id, updatedConnector.Id);
        return await _collection.FindOneAndReplaceAsync(filter, updatedConnector, cancellationToken: cancellationToken);
    }

    public async Task<Connector?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting connector with id {Id}.", id);
        return await _collection.FindOneAndDeleteAsync(c => c.Id == id, cancellationToken: cancellationToken);
    }
}
