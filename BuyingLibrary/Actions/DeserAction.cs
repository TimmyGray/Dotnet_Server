using BuyingLibrary.models.classes;
using BuyingLibrary.models.interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;

namespace BuyingLibrary.Actions;

public class DeserAction : IActions<Item>
{
    private const string LengthFieldName = "length";
    private const string TypeOfSignalFieldName = "typeofsignal";
    private readonly ILogger<DeserAction> _logger;

    public DeserAction(ILogger<DeserAction> logger)
    {
        _logger = logger;
    }

    public Item DeserBson(BsonDocument document)
    {
        if (document.Contains(LengthFieldName) && document.Contains(TypeOfSignalFieldName))
        {
            var coil = BsonSerializer.Deserialize<Coil>(document);
            _logger.LogDebug("Deserialised coil: {Coil}", coil);
            return coil;
        }

        var connector = BsonSerializer.Deserialize<Connector>(document);
        _logger.LogDebug("Deserialised connector: {Connector}", connector);
        return connector;
    }
}
