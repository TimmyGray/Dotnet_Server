using MongoDB.Bson;

namespace BuyingLibrary.models.interfaces;

public interface IActions<T>
{
    T DeserBson(BsonDocument document);
}
