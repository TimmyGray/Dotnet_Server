using BuyingLibrary.Actions;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class PricesController : ControllerBase
{
    private readonly PriceService _service;
    private readonly DeserAction _deser;

    public PricesController(PriceService service, DeserAction deser)
    {
        _service = service;
        _deser = deser;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<Price>> GetPrices(CancellationToken cancellationToken)
    {
        var documents = await _service.GetAsync(cancellationToken);
        var result = new List<Price>(documents.Count);

        foreach (var document in documents)
        {
            var item = _deser.DeserBson(document["itemofprice"].AsBsonDocument);
            var price = BsonSerializer.Deserialize<Price>(document);
            price.ItemOfPrice = item;
            result.Add(price);
        }

        return result;
    }
}
