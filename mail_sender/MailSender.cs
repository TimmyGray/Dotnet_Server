using BuyingLibrary.models.classes;
using MailKit.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace Aspnet_server.mail_sender
{
    internal class MailSender
    {
        private Order order;
        private string email;
        private string password;
        private string name;
        private string host;
        private int hostport;
        private readonly bool isRus;
        public MailSender(Order order, bool isRus)
        {

            this.order = order;
            this.isRus = isRus;
        }

        private void CheckSettings()
        {
            string path = Directory.GetCurrentDirectory()+ "\\mail_sender\\emailsettings.json";
            Console.WriteLine(path);

            using (StreamReader s = File.OpenText(path))
            {
                try
                {
                    JsonNode settingsjson = JsonNode.Parse(s.ReadToEnd());
                    if (settingsjson != null)
                    {
                        email = (string)settingsjson!["email"]!;
                        password = (string)settingsjson!["password"]!;
                        name = (string)settingsjson!["name"]!;
                        host = (string)settingsjson!["host"]!;
                        hostport = (int)settingsjson!["hostport"]!;
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }

        private string MakeBody(bool isRus)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine($"Здравствуйте,{order.client.Name}!");
            body.AppendLine($"Некоторое время назад, вы сделали заказ {order._id}:");
            foreach (var buy in order.Buys)
            {
                body.AppendLine(buy.ToString());
            }
            body.AppendLine("Об изменении статуса заказа вам придет отдельное сообщение");
            body.AppendLine($"Статус вашего заказа:{order.Status}");
            body.AppendLine($"Если у вас есть какие-то вопросы - пишите на почту {email}, с указанием номера заказа");
            return body.ToString();
        }

        internal void SendMail()
        {

            CheckSettings();
            if (email==null||password==null)
            {
                Console.WriteLine("Invalid email or password");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress($"{name}", email));
            message.To.Add(new MailboxAddress(order.client.Name,order.client.Email));
            message.Subject = $"{order.client.Name}, your order created!";
            message.Body = new TextPart("plain")
            {
                Text = MakeBody(isRus)
            
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

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

        }


        

    }

}
