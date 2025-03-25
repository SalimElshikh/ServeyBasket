namespace SurveyBasket.Settings;

public class MailSettings
{
    public string Mail { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Password{ get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    // Add This Code To Program
    // services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

}
