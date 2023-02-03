using Microsoft.AspNetCore.Mvc;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using MongoDB.Bson;
using BuyingLibrary.Actions;
using MongoDB.Bson.Serialization;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PricesController : ControllerBase
    {
        private readonly PriceService service;
        private readonly DeserAction deser;

        public PricesController(MongoContext context)
        {
            service = new PriceService(context);
            deser = new DeserAction();
        }

        [HttpGet]
        public async Task<List<Price>> GetPrices()
        {

            List<BsonDocument>? documents = await service.GetAsync();
            List<Price> result = new();
            
            foreach (var document in documents)
            {

                Item item = deser.DeserBson(document["itemofprice"].AsBsonDocument);
                Price price = BsonSerializer.Deserialize<Price>(document);
                price.Itemofprice = item;
                
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine(price.ToString());
                Console.ResetColor();
                
                result.Add(price);
            }
            

            return result;

        }
    }
}
