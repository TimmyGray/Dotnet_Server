using Aspnet_server.controllers;
using Aspnet_server.Tests.Fakes;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Mvc;

namespace Aspnet_server.Tests.Controllers;

public class BuysControllerTests
{
    [Fact]
    public async Task GetBuy_WithInvalidObjectId_ReturnsBadRequest()
    {
        var service = new InMemoryService<Buy>();
        var controller = new BuysController(service, null!);

        var result = await controller.GetBuy("invalid-id", CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
