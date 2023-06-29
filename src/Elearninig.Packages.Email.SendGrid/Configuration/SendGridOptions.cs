namespace Elearninig.Packages.Email.SendGrid.Configuration;

public sealed class SendGridOptions
{
    public string? ApiKey { get; set; }
    public SendGridFromOptions? From { get; set; }
}