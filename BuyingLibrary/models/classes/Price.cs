using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

[BsonIgnoreExtraElements]
public class Price
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("cost")]
    public double? Cost { get; set; }

    [BsonIgnore]
    public Item? ItemOfPrice { get; set; }

    public override string ToString() =>
        $"\n\t---Price---\n{Id}\n{Name}\n{Cost}\n{ItemOfPrice}\n";
}
