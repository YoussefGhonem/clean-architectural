using Elearninig.Packages.Email.Base.Models;

namespace Elearninig.Packages.Email.Base.Interfaces;

public interface ISingleEmailToMultipleRecipientsOptions
{
    public List<EmailAddressModel> To { get; }
    public string Subject { get; }
    public string Body { get; }
    public bool IsBodyHtml { get; }
}