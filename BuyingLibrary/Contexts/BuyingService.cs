using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class BuyingService : IService<Buy>
{
    private readonly IMongoCollection<Buy> _collection;
    private readonly ILogger<BuyingService> _logger;

    public BuyingService(MongoContext db, ILogger<BuyingService> logger)
    {
        _collection = db.BuysCollection;
        _logger = logger;
    }

    public async Task<List<Buy>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all non-custom buys.");
        return await _collection.Find(b => b.IsCustom == false).ToListAsync(cancellationToken);
    }

    public async Task<Buy?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching buy by id.");
        return await _collection.Find(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Buy> PostAsync(Buy newBuy, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting new buy.");
        await _collection.InsertOneAsync(newBuy, cancellationToken: cancellationToken);
        return newBuy;
    }

    public async Task<Buy?> PutAsync(Buy updatedBuy, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Replacing buy by id.");
        var filter = Builders<Buy>.Filter.Eq(b => b.Id, updatedBuy.Id);
        return await _collection.FindOneAndReplaceAsync(filter, updatedBuy, cancellationToken: cancellationToken);
    }

    public async Task<Buy?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting buy by id.");
        return await _collection.FindOneAndDeleteAsync(b => b.Id == id, cancellationToken: cancellationToken);
    }
}
