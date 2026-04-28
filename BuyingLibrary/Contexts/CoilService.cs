using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class CoilService : IService<Coil>
{
    private readonly IMongoCollection<Coil> _collection;
    private readonly ILogger<CoilService> _logger;

    public CoilService(MongoContext context, ILogger<CoilService> logger)
    {
        _collection = context.CoilsCollection;
        _logger = logger;
    }

    public async Task<List<Coil>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all coils.");
        return await _collection.Find(FilterDefinition<Coil>.Empty).ToListAsync(cancellationToken);
    }

    public async Task<Coil?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching coil with id {Id}.", id);
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Coil> PostAsync(Coil coil, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting new coil: {Name}.", coil.Name);
        await _collection.InsertOneAsync(coil, cancellationToken: cancellationToken);
        return coil;
    }

    public async Task<Coil?> PutAsync(Coil updatedCoil, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Replacing coil with id {Id}.", updatedCoil.Id);
        var filter = Builders<Coil>.Filter.Eq(c => c.Id, updatedCoil.Id);
        return await _collection.FindOneAndReplaceAsync(filter, updatedCoil, cancellationToken: cancellationToken);
    }

    public async Task<Coil?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting coil with id {Id}.", id);
        return await _collection.FindOneAndDeleteAsync(c => c.Id == id, cancellationToken: cancellationToken);
    }
}
