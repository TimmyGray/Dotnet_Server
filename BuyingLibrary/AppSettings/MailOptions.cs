namespace BuyingLibrary.AppSettings;

public class MailOptions
{
    public const string EmailSettings = "EmailSettings";

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Name { get; set; } = string.Empty;
}
