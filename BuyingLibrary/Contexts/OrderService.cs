using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class OrderService : IService<Order>
{
    private readonly IMongoCollection<Order> _collection;
    private readonly ILogger<OrderService> _logger;

    public OrderService(MongoContext db, ILogger<OrderService> logger)
    {
        _collection = db.OrderCollection;
        _logger = logger;
    }

    public async Task<List<Order>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all orders.");
        return await _collection.Find(FilterDefinition<Order>.Empty).ToListAsync(cancellationToken);
    }

    public async Task<List<Order>> GetByClientAsync(string clientId, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching orders for client.");
        return await _collection
            .Find(o => o.Client != null && o.Client.Id == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByClientAndOrderAsync(
        string clientId,
        string orderId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching order for client.");
        return await _collection
            .Find(o => o.Client != null && o.Client.Id == clientId && o.Id == orderId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Order?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching order with id {Id}.", id);
        return await _collection.Find(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Order> PostAsync(Order newOrder, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inserting new order.");
        foreach (var buy in newOrder.Buys)
        {
            if (string.IsNullOrEmpty(buy.Id))
            {
                buy.Id = ObjectId.GenerateNewId().ToString();
                if (buy.Image is not null)
                {
                    buy.Image.Id = ObjectId.GenerateNewId().ToString();
                }
                buy.IsCustom = true;
                _logger.LogDebug("Assigned new id to custom buy.");
            }
        }

        await _collection.InsertOneAsync(newOrder, cancellationToken: cancellationToken);
        return newOrder;
    }

    public async Task<Order?> PutAsync(Order updatedOrder, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Replacing order by id.");
        var filter = Builders<Order>.Filter.Eq(o => o.Id, updatedOrder.Id);
        return await _collection.FindOneAndReplaceAsync(filter, updatedOrder, cancellationToken: cancellationToken);
    }

    public async Task<Order?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting order by id.");
        return await _collection.FindOneAndDeleteAsync(o => o.Id == id, cancellationToken: cancellationToken);
    }
}
