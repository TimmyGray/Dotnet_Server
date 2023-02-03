using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;


namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ClientsController:ControllerBase
    {
        private readonly IService<Client> service;

        public ClientsController(MongoContext context)
        {

            service = new ClientService(context);

        }

        [HttpGet("id:length(24)")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {

            var client = await service.GetAsync(id);
            
            if (client!=null)
            {
                return Ok(client);
            }

            return BadRequest();

        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client newclient) 
        {
            if (newclient._id!="")
            {
                var client = await service.GetAsync(newclient._id);
                if (client != null)
                {
                    return Ok(client);
                    //return BadRequest(client);
                }
            }


            return await service.PostAsync(newclient);
        
        }

        [HttpDelete("id:length(24)")]
        public async Task<ActionResult<Client>> DeleteClient(string id)
        {

            return await service.DeleteAsync(id);

        }

    }
}
