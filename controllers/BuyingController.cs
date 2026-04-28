using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class BuysController : ControllerBase
{
    private readonly IService<Buy> _service;
    private readonly ImageService _imageService;

    public BuysController(IService<Buy> service, ImageService imageService)
    {
        _service = service;
        _imageService = imageService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<List<Buy>> GetBuys(CancellationToken cancellationToken) =>
        _service.GetAsync(cancellationToken);

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Buy>> GetBuy(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var buy = await _service.GetAsync(id, cancellationToken);
        return buy is null ? NotFound() : Ok(buy);
    }

    [HttpGet("image/{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetImage(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        Response.ContentType = "application/octet-stream";
        await _imageService.GetOneAsync(id, Response.Body, cancellationToken);
        return new EmptyResult();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Buy>> PostBuy([FromBody] Buy newBuy, CancellationToken cancellationToken)
    {
        var result = await _service.PostAsync(newBuy, cancellationToken);
        return CreatedAtAction(nameof(GetBuy), new { id = result.Id }, result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Buy>> PutBuy([FromBody] Buy updatedBuy, CancellationToken cancellationToken)
    {
        var result = await _service.PutAsync(updatedBuy, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Buy>> DeleteBuy(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var result = await _service.DeleteAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
