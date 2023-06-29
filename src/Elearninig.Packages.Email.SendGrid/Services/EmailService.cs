using Elearninig.Packages.Email.Base.EmailOptions;
using Elearninig.Packages.Email.Base.Interfaces;
using Elearninig.Packages.Email.SendGrid.Configuration;
using Elearninig.Packages.Email.SendGrid.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Elearninig.Packages.Email.SendGrid.Services;

public class EmailService : IEmailService
{
    private readonly SendGridOptions _sendGridConfig;

    public EmailService(IConfiguration configuration)
    {
        _sendGridConfig = configuration.GetSendGridConfig();
    }

    public async Task SendEmail(SingleEmailOptions options)
    {
        var client = new SendGridClient(_sendGridConfig.ApiKey);
        var from = new EmailAddress(_sendGridConfig.From?.Email, _sendGridConfig.From?.Name);
        var to = new EmailAddress(options.To.Email, options.To.Name);

        var msg = MailHelper.CreateSingleEmail(from, to, options.Subject, options.PlainTextContent,
            options.HtmlContent);

        var response = await client.SendEmailAsync(msg);
        await SendGridException.ThrowExceptionOnError(response);
    }

    public async Task SendEmail(SingleEmailToMultipleRecipientsOptions options)
    {
        var client = new SendGridClient(_sendGridConfig.ApiKey);
        var from = new EmailAddress(_sendGridConfig.From?.Email, _sendGridConfig.From?.Name);
        var tos = options.To.Select(x => new EmailAddress(x.Email, x.Name)).ToList();

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, options.Subject,
            options.PlainTextContent, options.HtmlContent);

        // Mail Helper.
        var response = await client.SendEmailAsync(msg);

        await SendGridException.ThrowExceptionOnError(response);
    }

    public Task SendEmailWithAttachments(SingleEmailOptions options, List<IFormFile> attachments)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachments(SingleEmailToMultipleRecipientsOptions options, List<IFormFile> attachments)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachmentsAsStreams(SingleEmailOptions options, List<MemoryStream> attachments)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachmentsAsStreams(SingleEmailToMultipleRecipientsOptions options, List<MemoryStream> attachments)
    {
        throw new NotImplementedException();
    }
}