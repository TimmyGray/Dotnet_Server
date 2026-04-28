using BuyingLibrary.AppSettings;
using BuyingLibrary.models.classes;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace Aspnet_server.mail_sender;

public sealed class MailSender : IMailSender
{
    private readonly ILogger<MailSender> _logger;
    private readonly MailOptions _options;

    public MailSender(IOptions<MailOptions> options, ILogger<MailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    private bool IsConfigured() =>
        !string.IsNullOrWhiteSpace(_options.Email) &&
        !string.IsNullOrWhiteSpace(_options.Password) &&
        !string.IsNullOrWhiteSpace(_options.Host) &&
        _options.Port > 0;

    private string MakeBody(Order order, bool isRus)
    {
        var body = new StringBuilder();

        if (isRus)
        {
            body.AppendLine($"Здравствуйте, {order.Client?.Name}!");
            body.AppendLine($"Некоторое время назад вы сделали заказ {order.Id}:");
            foreach (var buy in order.Buys)
            {
                body.AppendLine(buy.ToString());
            }

            body.AppendLine("Об изменении статуса заказа вам придет отдельное сообщение.");
            body.AppendLine($"Статус вашего заказа: {order.Status}");
            body.AppendLine($"Если у вас есть вопросы — пишите на почту {_options.Email} с указанием номера заказа.");
            return body.ToString();
        }

        body.AppendLine($"Hello, {order.Client?.Name}!");
        body.AppendLine($"You created order {order.Id}:");
        foreach (var buy in order.Buys)
        {
            body.AppendLine(buy.ToString());
        }

        body.AppendLine("You will receive a notification when order status changes.");
        body.AppendLine($"Current order status: {order.Status}");
        body.AppendLine($"If you have questions, write to {_options.Email} with your order id.");
        return body.ToString();
    }

    public async Task SendOrderCreatedAsync(Order order, bool isRus, CancellationToken cancellationToken = default)
    {
        if (!IsConfigured())
        {
            _logger.LogWarning("Email settings are missing. Skipping email notification.");
            return;
        }

        if (string.IsNullOrWhiteSpace(order.Client?.Email))
        {
            _logger.LogWarning("Order has no client email. Skipping email notification.");
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.Name, _options.Email));
        message.To.Add(new MailboxAddress(order.Client.Name, order.Client.Email));
        message.Subject = $"{order.Client.Name}, your order has been created";
        message.Body = new TextPart("plain") { Text = MakeBody(order, isRus) };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            await client.AuthenticateAsync(_options.Email, _options.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Order creation email sent.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send order creation email.");
        }
    }
}
