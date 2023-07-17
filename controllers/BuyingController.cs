using BuyingLibrary;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Aspnet_server.controllers
{
    
    [ApiController]
    [Route("/[controller]")]
    public class BuysController : ControllerBase
    {
        private readonly IService<Buy> service;
        private readonly ImageService imageService;

        public BuysController(MongoContext context)
        {
            service = new BuyingService(context);
            imageService = new ImageService(context);
        }

        [HttpGet]
        public async Task<List<Buy>> GetBuys()
        {
            //var images = await imageService.GetAllAsync();
            var buys = await service.GetAsync();
            foreach (var buy in buys)
            {
                //buy.Image.Data = images.FirstOrDefault(i => i._id == buy.Image._id).Data;

                //Console.WriteLine($"{buy.Image.Data.Length}");

                buy.Count = 1;
                Console.WriteLine(buy.ToString());
            }
            return buys;

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

        [Route("getimage/{id}")]
        public async Task GetImage(string id)
        {
            Console.WriteLine("Gt Image");
            await imageService.GetOne(id,Response.Body);
            //return Response;
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
