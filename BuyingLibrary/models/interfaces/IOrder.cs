using BuyingLibrary.models.classes;
using BuyingLibrary.models.enums;

namespace BuyingLibrary.models.interfaces;

public interface IOrder
{
    string? Id { get; set; }

    Client? Client { get; set; }

    string? Name { get; set; }

    DateTime Created { get; }

    OrderStatus Status { get; set; }

    List<Buy> Buys { get; set; }
}
