using BuyingLibrary.models.classes;

namespace Aspnet_server.mail_sender;

public interface IMailSender
{
    Task SendOrderCreatedAsync(Order order, bool isRus, CancellationToken cancellationToken = default);
}
