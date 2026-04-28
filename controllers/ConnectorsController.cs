using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class ConnectorsController : ControllerBase
{
    private readonly IService<Connector> _service;

    public ConnectorsController(IService<Connector> service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<List<Connector>> GetConnectors(CancellationToken cancellationToken) =>
        _service.GetAsync(cancellationToken);
}
