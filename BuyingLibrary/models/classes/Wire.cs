using BuyingLibrary.models.classes;
using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Wire
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("length")]
    public double? Length { get; set; }

    [BsonElement("firstconn")]
    public Connector[]? FirstConnector { get; set; }

    [BsonElement("secondconn")]
    public Connector[]? SecondConnector { get; set; }

    [BsonElement("numberofconnectors")]
    public int? NumberOfConnectors { get; set; }
}
