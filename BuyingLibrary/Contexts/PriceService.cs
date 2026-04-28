using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class PriceService : IService<BsonDocument>
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly ILogger<PriceService> _logger;

    public PriceService(MongoContext context, ILogger<PriceService> logger)
    {
        _collection = context.PricesCollection;
        _logger = logger;
    }

    public async Task<List<BsonDocument>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all price documents.");
        return await _collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync(cancellationToken);
    }

    public async Task<BsonDocument?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching price document with id {Id}.", id);
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<BsonDocument> PostAsync(BsonDocument document, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting new price document.");
        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return document;
    }

    public async Task<BsonDocument?> PutAsync(BsonDocument document, CancellationToken cancellationToken = default)
    {
        var id = document["_id"].AsObjectId;
        _logger.LogInformation("Replacing price document with id {Id}.", id);
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
        return await _collection.FindOneAndReplaceAsync(filter, document, cancellationToken: cancellationToken);
    }

    public async Task<BsonDocument?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting price document with id {Id}.", id);
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        return await _collection.FindOneAndDeleteAsync(filter, cancellationToken: cancellationToken);
    }
}
