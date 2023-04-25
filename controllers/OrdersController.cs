using BuyingLibrary.Contexts;
using Microsoft.AspNetCore.Mvc;
using BuyingLibrary.models.classes;
using BuyingLibrary.models.interfaces;
using MongoDB.Driver;
using Aspnet_server.mail_sender;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class OrdersController:ControllerBase
    {
        private readonly OrderService service;
        private readonly MailSender mailsender;

        public OrdersController(MongoContext context, MailSender _mailsender)
        {
            Console.WriteLine("Order Controller");
            Console.WriteLine($"EmailMail sender setup - {_mailsender.Setup}");
            mailsender = _mailsender;
            service = new OrderService(context);
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders()
        {

            return await service.GetAsync();

        }

        [HttpGet("{clientid:length(24)}")]
        
        public async Task<ActionResult<List<Order>>> GetOrders(string clientid)
        {

            var orders = await service.GetAsync(clientid);
            if (orders!=null)
            {
                foreach (var o in orders)
                {

                    Console.WriteLine(o.ToString());

                }
                return Ok(orders);
            }

            return BadRequest();

        }

        [HttpGet("{clientid:length(24)}/{orderid:length(24)}")]
        public async Task<ActionResult<Order>> GetAsync(string clientid,string orderid)
        {
            
            var order = await service.GetAsync(clientid, orderid);
            if (order!=null)
            {
                Console.WriteLine(order.ToString());
                return Ok(order);
            }

            return BadRequest();

        }
        

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order neworder)
        {
            Console.WriteLine("Post order controller");

            if (neworder == null)
            {
                return NoContent();
            }


            var result = await service.PostAsync(neworder);
        
            Console.WriteLine("Try to send mail...");
            mailsender.SendMail(neworder,false);


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
