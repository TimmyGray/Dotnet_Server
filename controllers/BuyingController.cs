using BuyingLibrary;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class BuysController : ControllerBase
    {
        private IService<Buy> service;

        public BuysController(MongoContext context)
        {
            service = new BuyingService(context);
        }

        [HttpGet]
        public async Task<List<Buy>> GetBuys()
        {

            return await service.GetAsync();

        }

        [HttpGet("id:length(24)")]
        public async Task<ActionResult<Buy>> GetBuy(string id)
        {

            var buy = await service.GetAsync(id);
            if (buy != null)
            {
                return buy;
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<ActionResult<Buy>> PostBuy(Buy newbuy)
        {
            if (newbuy == null)
            {
                return NoContent();
            }

            var result = await service.PostAsync(newbuy);

            return result;

        }

        [HttpPut]
        public async Task<ActionResult<Buy>> PutBuy(Buy newbuy)
        {

            if (newbuy == null)
            {
                return NoContent();
            }

            var result = await service.PutAsync(newbuy);

            if (result == null)
            {
                return NotFound();
            }

            return result;


        }

        [HttpDelete("id:length(24)")]
        public async Task<ActionResult<Buy>> DeleteBuy(string id)
        {

            var result = await service.DeleteAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;

        }


    }
}
