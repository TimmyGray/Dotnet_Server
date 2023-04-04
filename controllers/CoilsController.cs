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
        public async Task<List<Coil>> Get()
        {
            
            var Coils = await service.GetAsync();
            
            foreach (var coil in Coils) 
            {

                coil.Length = 1;

            }

            return Coils;

        }


    }
}
