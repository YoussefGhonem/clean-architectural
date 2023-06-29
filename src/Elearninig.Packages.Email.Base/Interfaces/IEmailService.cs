using Elearninig.Packages.Email.Base.EmailOptions;
using Microsoft.AspNetCore.Http;

namespace Elearninig.Packages.Email.Base.Interfaces;

public interface IEmailService
{
    Task SendEmail(SingleEmailOptions options);
    Task SendEmail(SingleEmailToMultipleRecipientsOptions options);
    Task SendEmailWithAttachments(SingleEmailToMultipleRecipientsOptions options, List<IFormFile> attachments);
    Task SendEmailWithAttachments(SingleEmailOptions options, List<IFormFile> attachments);
    Task SendEmailWithAttachmentsAsStreams(SingleEmailOptions options, List<MemoryStream> attachments);
    Task SendEmailWithAttachmentsAsStreams(SingleEmailToMultipleRecipientsOptions options, List<MemoryStream> attachments);

}