using Ardalis.GuardClauses;
using Elearninig.Packages.Email.Base.Extensions;
using Elearninig.Packages.Email.Base.Interfaces;
using Elearninig.Packages.Email.Base.Models;

namespace Elearninig.Packages.Email.Base.EmailOptions;

public class SingleEmailOptions : ISingleEmailOptions
{
    public EmailAddressModel To { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public bool IsBodyHtml { get; private set; }

    public string? PlainTextContent
    {
        get { return IsBodyHtml ? null : Body; }
    }

    public string? HtmlContent
    {
        get { return IsBodyHtml ? Body : null; }
    }

    public SingleEmailOptions(EmailAddressModel to, string subject, string textBody)
    {
        To = to;
        Subject = Guard.Against.NullOrWhiteSpace(subject, nameof(subject));
        Body = Guard.Against.NullOrWhiteSpace(textBody, nameof(textBody));
        IsBodyHtml = textBody.Contains("<div") && textBody.Contains("</div");
    }

    public SingleEmailOptions(EmailAddressModel to, string subject, string htmlBody,
        List<TemplatePlaceholderModel> placeholders)
    {
        var body = htmlBody.ReplacePlaceholders(placeholders);
        To = to;
        Subject = Guard.Against.NullOrWhiteSpace(subject, nameof(subject));
        Body = Guard.Against.NullOrWhiteSpace(body, nameof(body));
        IsBodyHtml = true;
    }
}