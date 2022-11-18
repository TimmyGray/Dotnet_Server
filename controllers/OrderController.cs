using BuyingLibrary.Contexts;
using Microsoft.AspNetCore.Mvc;
using BuyingLibrary.models.classes;
using BuyingLibrary.models.interfaces;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class OrdersController:ControllerBase
    {
        private IService<Order> service;

        public OrdersController(MongoContext context)
        {
            service = new OrderService(context);
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders()
        {

            return await service.GetAsync();

        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {

            var order = await service.GetAsync(id);
            if (order != null)
            {
                return order;
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order neworder)
        {
            if (neworder == null)
            {
                return NoContent();
            }

            var result = await service.PostAsync(neworder);

            return result;

        }

        [HttpPut]
        public async Task<ActionResult<Order>> PutOrder(Order neworder)
        {

            if (neworder == null)
            {
                return NoContent();
            }

            var result = await service.PutAsync(neworder);

            if (result==null)
            {
                return NotFound();
            }
            
            return result;


        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<Order>> DeleteOrder(string id)
        {

            var result = await service.DeleteAsync(id);
            if (result==null)
            {
                return NotFound();
            }
            return result;

        }


    }
}
