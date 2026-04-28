using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Item
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("type")]
    public string? Type { get; set; }
}
