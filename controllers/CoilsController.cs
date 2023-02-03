using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CoilsController:ControllerBase
    {
        private readonly IService<Coil> service;

        public CoilsController(MongoContext context)
        {

            service = new CoilService(context);

        }

        [HttpGet]
        public async Task<List<string>> Get()
        {
            
            var Coils = await service.GetAsync();
            List<string> coils = new List<string>();
            
            foreach (var coil in Coils) 
            {
            
                coils.Add($"{coil.Name}-{coil.Type}");
            }

            return coils;

        }


    }
}
