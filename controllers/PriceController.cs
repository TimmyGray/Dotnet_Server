using Microsoft.AspNetCore.Mvc;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PriceController : ControllerBase
    {
        IService<Price> service;
        public PriceController(MongoContext context)
        {
            service = new PriceService(context);
        }

        [HttpGet]
        public async Task<List<Price>> GetPrices()
        {

            return await service.GetAsync();

        }
    }
}
