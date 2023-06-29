namespace Elearninig.Packages.Email.Smtp.Config;

public sealed class SmtpOptions
{
    public string? Server { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public bool IgnoreCertificateErrors { get; set; }
    public SmtpFromOptions? From { get; set; }
}