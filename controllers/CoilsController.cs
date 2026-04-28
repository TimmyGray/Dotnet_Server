using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class CoilsController : ControllerBase
{
    private readonly IService<Coil> _service;

    public CoilsController(IService<Coil> service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<List<Coil>> Get(CancellationToken cancellationToken) =>
        _service.GetAsync(cancellationToken);
}
