using BuyingLibrary.models.enums;
using BuyingLibrary.models.interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace BuyingLibrary.models.classes;

public class Order : IOrder
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("client")]
    public Client? Client { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("created")]
    public DateTime Created { get; } = DateTime.UtcNow;

    [BsonElement("status")]
    public OrderStatus Status { get; set; } = OrderStatus.UnderConsideration;

    [BsonElement("listofbuys")]
    public List<Buy> Buys { get; set; } = [];

    public override string ToString() =>
        $"\n\t---Order---\n" +
        $"id:{Id}\n" +
        $"name:{Name}\n" +
        $"status:{Status}\n" +
        $"date of create:{Created}\n" +
        $"client name:{Client?.Name}\n" +
        $"client email:{Client?.Email}\n";
}
