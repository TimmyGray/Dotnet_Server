using Aspnet_server.mail_sender;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Aspnet_server.controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;
    private readonly IMailSender _mailSender;

    public OrdersController(OrderService service, IMailSender mailSender)
    {
        _service = service;
        _mailSender = mailSender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<List<Order>> GetOrders(CancellationToken cancellationToken) =>
        _service.GetAsync(cancellationToken);

    [HttpGet("{clientId:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Order>>> GetOrdersByClient(string clientId, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(clientId, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["clientId"] = ["Invalid ObjectId format"] }));
        }

        var orders = await _service.GetByClientAsync(clientId, cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{clientId:length(24)}/{orderId:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetByClientAndOrder(string clientId, string orderId, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(clientId, out _) || !ObjectId.TryParse(orderId, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var order = await _service.GetByClientAndOrderAsync(clientId, orderId, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Order>> PostOrder([FromBody] Order newOrder, CancellationToken cancellationToken)
    {
        var result = await _service.PostAsync(newOrder, cancellationToken);
        await _mailSender.SendOrderCreatedAsync(result, isRus: false, cancellationToken);
        return CreatedAtAction(nameof(GetByClientAndOrder), new { clientId = result.Client?.Id, orderId = result.Id }, result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> PutOrder([FromBody] Order updatedOrder, CancellationToken cancellationToken)
    {
        var result = await _service.PutAsync(updatedOrder, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> DeleteOrder(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["id"] = ["Invalid ObjectId format"] }));
        }

        var result = await _service.DeleteAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
