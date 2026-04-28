global using MongoDB.Bson;
global using MongoDB.Driver;
global using MongoDB.Driver.GridFS;

using BuyingLibrary.AppSettings;
using BuyingLibrary.models.classes;
using Microsoft.Extensions.Options;

namespace BuyingLibrary.Contexts;

public sealed class MongoContext
{
    private readonly IMongoCollection<Order> _orderCollection;
    private readonly IMongoCollection<Buy> _buysCollection;
    private readonly IMongoCollection<BsonDocument> _pricesCollection;
    private readonly IMongoCollection<Connector> _connectorsCollection;
    private readonly IMongoCollection<Coil> _coilsCollection;
    private readonly IMongoCollection<Client> _clientsCollection;
    private readonly GridFSBucket _imageStore;

    public MongoContext(IOptions<DataBaseOptions> settings)
    {
        var client = new MongoClient(settings.Value.DataBaseConnection);
        var db = client.GetDatabase(settings.Value.DataBase);

        _orderCollection = db.GetCollection<Order>("orders");
        _buysCollection = db.GetCollection<Buy>("buys");
        _pricesCollection = db.GetCollection<BsonDocument>("prices");
        _connectorsCollection = db.GetCollection<Connector>("connectors");
        _coilsCollection = db.GetCollection<Coil>("coils");
        _clientsCollection = db.GetCollection<Client>("clients");
        _imageStore = new GridFSBucket(db, new GridFSBucketOptions { BucketName = "imagestore" });
    }

    internal IMongoCollection<Order> OrderCollection => _orderCollection;
    internal IMongoCollection<Buy> BuysCollection => _buysCollection;
    internal IMongoCollection<BsonDocument> PricesCollection => _pricesCollection;
    internal IMongoCollection<Coil> CoilsCollection => _coilsCollection;
    internal IMongoCollection<Client> ClientsCollection => _clientsCollection;
    internal IMongoCollection<Connector> ConnectorsCollection => _connectorsCollection;
    internal GridFSBucket ImageStore => _imageStore;
}
