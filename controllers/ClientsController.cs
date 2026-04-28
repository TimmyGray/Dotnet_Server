using Aspnet_server.Contracts;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IService<Client> _service;

    public ClientsController(IService<Client> service)
    {
        _service = service;
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Client>> GetClient(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var client = await _service.GetAsync(id, cancellationToken);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Client>> PostClient([FromBody] ClientUpsertRequest request, CancellationToken cancellationToken)
    {
        var newClient = new Client
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email
        };

        var result = await _service.PostAsync(newClient, cancellationToken);
        return CreatedAtAction(nameof(GetClient), new { id = result.Id }, result);
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Client>> DeleteClient(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var result = await _service.DeleteAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
