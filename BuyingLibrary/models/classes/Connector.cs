using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Connector : Item
{
    [BsonElement("count")]
    public int? Count { get; set; }

    public override string ToString() =>
        $"\n\t---Connector---\n{Id}\n{Name}\n{Type}\n";
}
