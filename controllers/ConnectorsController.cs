using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class ConnectorsController:ControllerBase
    {
        private readonly IService<Connector> service;
        
        public ConnectorsController(MongoContext context)
        {
            service = new ConnectorService(context);
        }

        [HttpGet]
        public async Task<List<Connector>> GetConnectors()
        {
            
            var Connectors = await service.GetAsync();
            foreach(var con in Connectors)
            {
                con.Count = 1;
            }

            return Connectors;

        }

    }
}
