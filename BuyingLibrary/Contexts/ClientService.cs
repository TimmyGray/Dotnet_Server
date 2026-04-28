using BuyingLibrary.models.classes;
using Microsoft.Extensions.Logging;

namespace BuyingLibrary.Contexts;

public sealed class ClientService : IService<Client>
{
    private readonly IMongoCollection<Client> _collection;
    private readonly ILogger<ClientService> _logger;

    public ClientService(MongoContext db, ILogger<ClientService> logger)
    {
        _collection = db.ClientsCollection;
        _logger = logger;
    }

    public async Task<List<Client>> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all clients.");
        return await _collection.Find(FilterDefinition<Client>.Empty).ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching client with id {Id}.", id);
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Client> PostAsync(Client client, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating client with email {Email} if it does not already exist.", client.Email);
        var existing = await _collection.Find(c => c.Email == client.Email)
                                         .FirstOrDefaultAsync(cancellationToken);
        if (existing is not null)
        {
            _logger.LogDebug("Client already exists, returning existing record.");
            return existing;
        }

        await _collection.InsertOneAsync(client, cancellationToken: cancellationToken);
        return client;
    }

    public async Task<Client?> PutAsync(Client updatedClient, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Replacing client with id {Id}.", updatedClient.Id);
        var filter = Builders<Client>.Filter.Eq(c => c.Id, updatedClient.Id);
        return await _collection.FindOneAndReplaceAsync(filter, updatedClient, cancellationToken: cancellationToken);
    }

    public async Task<Client?> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting client with id {Id}.", id);
        return await _collection.FindOneAndDeleteAsync(c => c.Id == id, cancellationToken: cancellationToken);
    }
}
