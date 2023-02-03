using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers
{
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
        public async Task<List<string>> GetConnectors()
        {
            List<string> connectors = new List<string>();
            
            var Connectors = await service.GetAsync();
            foreach(var con in Connectors)
            {
                connectors.Add($"{con.Name}-{con.Type}");
            }

            return connectors;

        }

    }
}
