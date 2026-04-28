namespace BuyingLibrary.AppSettings;

public class DataBaseOptions
{
    public const string DataBaseSettings = "DataBaseSettings";

    public string DataBaseConnection { get; set; } = string.Empty;

    public string DataBase { get; set; } = string.Empty;
}
