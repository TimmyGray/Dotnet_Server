using Aspnet_server.Contracts;
using Aspnet_server.controllers;
using Aspnet_server.Tests.Fakes;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.Tests.Controllers;

public class ClientsControllerTests
{
    [Fact]
    public async Task GetClient_WithInvalidObjectId_ReturnsBadRequest()
    {
        var service = new InMemoryService<Client>();
        var controller = new ClientsController(service);

        var result = await controller.GetClient("not-an-objectid", CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task PostClient_ReturnsCreatedAtAction()
    {
        var service = new InMemoryService<Client>();
        var controller = new ClientsController(service);

        var request = new ClientUpsertRequest
        {
            Id = "507f1f77bcf86cd799439011",
            Name = "John",
            Email = "john@example.com"
        };

        var result = await controller.PostClient(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ClientsController.GetClient), created.ActionName);

        var payload = Assert.IsType<Client>(created.Value);
        Assert.Equal(request.Name, payload.Name);
        Assert.Equal(request.Email, payload.Email);
    }
}
