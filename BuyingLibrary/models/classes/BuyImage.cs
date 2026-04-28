using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

[BsonIgnoreExtraElements]
public class BuyImage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("type")]
    public string? Type { get; set; }

    [BsonElement("size")]
    public int Size { get; set; }

    [BsonIgnore]
    public byte[]? Data { get; set; }
}
