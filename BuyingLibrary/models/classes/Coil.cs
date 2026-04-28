using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Coil : Item
{
    [BsonElement("length")]
    public double Length { get; set; }

    [BsonElement("typeofsignal")]
    public string? TypeOfSignal { get; set; }

    public override string ToString() =>
        $"\n\t---Coil---\n{Id}\n{Name}\n{Type}\n{Length}\n{TypeOfSignal}\n";
}
