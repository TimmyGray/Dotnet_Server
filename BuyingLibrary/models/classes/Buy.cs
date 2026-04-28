using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Buy
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("cost")]
    public double Cost { get; set; }

    [BsonElement("item")]
    public string? Item { get; set; }

    [BsonElement("itemid")]
    public string? ItemId { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }

    [BsonElement("image")]
    public BuyImage? Image { get; set; }

    [BsonElement("custom")]
    public bool IsCustom { get; set; }

    public override string ToString() =>
        $"\n\t-----Buy-----\nname - {Name}\ndescription - {Description}\ncost - {Cost}\nitem - {Item}\nis custom - {IsCustom}\n";
}
