using BuyingLibrary.AppSettings;
using BuyingLibrary.models.classes;
using MailKit.Net;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace Aspnet_server.mail_sender
{
    public class MailSender
    {
        private IOptions<MailOptions> _options;
        private string email;
        private string password;
        private string name;
        private string host;
        private int hostport;
        public bool Setup { get; }

        public MailSender(IOptions<MailOptions> options )
        {
            _options = options;
            //Console.WriteLine($"{_options.Value.Email}");
            //Console.WriteLine($"{_options.Value.Host}");
            //Console.WriteLine($"{_options.Value.Port}");
            //Console.WriteLine($"{_options.Value.Name}"); mn

            if ( _options.Value.Email=="" ) {
                Setup = false;
                Console.WriteLine("Settings not read!");
            }
            else
            {
                Setup = true;
                email = _options.Value.Email;
                password = _options.Value.Password;
                host = _options.Value.Host;
                hostport = Convert.ToInt32(_options.Value.Port);
                name = _options.Value.Name;

                Console.WriteLine("The mailsender created!");

            }
        }

        private string MakeBody(Order order,bool isRus)
        {
            StringBuilder body = new StringBuilder();

            if (isRus) {

                body.AppendLine($"Здравствуйте,{order.Client.Login}!");
                body.AppendLine($"Некоторое время назад, вы сделали заказ {order._id}:");
                foreach (var buy in order.Buys)
                {
                    body.AppendLine(buy.ToString());
                }
                body.AppendLine("Об изменении статуса заказа вам придет отдельное сообщение");
                body.AppendLine($"Статус вашего заказа:{order.Status}");
                body.AppendLine($"Если у вас есть какие-то вопросы - пишите на почту {email}, с указанием номера заказа");

            }
            else
            {
                body.AppendLine($"Hello,{order.Client.Login}!");
                body.AppendLine($"Some times ago you make an order: {order._id}:");
                foreach (var buy in order.Buys)
                {
                    body.AppendLine(buy.ToString());
                }
                body.AppendLine("When the status order has changed - you will receive a message on your email");
                body.AppendLine($"Current status order:{order.Status}");
                body.AppendLine($"If you have some questions - write on {email} with order id");

            }
            return body.ToString();
        }

        internal void SendMail(Order order, bool isRus)
        {
            Console.WriteLine("Mail send method");
            //CheckSettings();
            if (!Setup)
            {
                return;
            }
            Console.WriteLine("All right!");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress($"{name}", email));
            message.To.Add(new MailboxAddress(order.Client.Login,order.Client.Email));
            message.Subject = $"{order.Client.Login}, your order created!";
            message.Body = new TextPart("plain")
            {
                Text = MakeBody(order,isRus)
            
            };

            try
            {
                using (SmtpClient client = new SmtpClient())
                {

                    client.Connect($"{host}", hostport, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.Authenticate(email, password);
                    client.Send(message);
                    client.Disconnect(true);

                }
                Console.WriteLine("The message sended!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

        }


        

    }

}
