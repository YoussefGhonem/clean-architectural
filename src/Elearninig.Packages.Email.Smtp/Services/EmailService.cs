using Elearninig.Packages.Email.Base.EmailOptions;
using Elearninig.Packages.Email.Base.Interfaces;
using Elearninig.Packages.Email.Base.Models;
using Elearninig.Packages.Email.Smtp.Config;
using Elearninig.Packages.Email.Smtp.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Elearninig.Packages.Email.Smtp.Services;

public class EmailService : IEmailService
{
    private readonly SmtpOptions _smtpConfig;

    public EmailService(IConfiguration configuration)
    {
        _smtpConfig = configuration.GetSmtpConfig();
    }

    public async Task SendEmail(SingleEmailOptions options)
    {
        //string to = options.To.Email; //// for Mimekit
        var to = new List<EmailAddressModel>
        {
            options.To
        };
        await Send(to, options.Subject, options.Body, options.IsBodyHtml);
    }

    public async Task SendEmail(SingleEmailToMultipleRecipientsOptions options)
    {
        //string to = string.Join(",", options.To.Select(x => x.Email)); //// for Mimekit
        await Send(options.To, options.Subject, options.Body, options.IsBodyHtml);
    }

    public async Task SendEmailWithAttachments(SingleEmailOptions options, List<IFormFile> attachments)
    {
        var to = new List<EmailAddressModel>
        {
            options.To
        };
        await SendWithAttachments(to, options.Subject, options.Body, options.IsBodyHtml, attachments);
    }

    public async Task SendEmailWithAttachments(SingleEmailToMultipleRecipientsOptions options, List<IFormFile> attachments)
    {
        await SendWithAttachments(options.To, options.Subject, options.Body, options.IsBodyHtml, attachments);
    }

    public async Task SendEmailWithAttachmentsAsStreams(SingleEmailOptions options, List<MemoryStream> attachments)
    {
        var to = new List<EmailAddressModel>
        {
            options.To
        };
        await SendWithAttachmentsAsStreams(to, options.Subject, options.Body, options.IsBodyHtml, attachments);
    }

    private async Task SendWithAttachmentsAsStreams(List<EmailAddressModel> to, string subject, string body, bool isBodyHtml, List<MemoryStream> attachments)
    {
        try
        {
            // Using Microsoft Package

            var smtpClient = new SmtpClient(_smtpConfig.Server)
            {
                Port = _smtpConfig.Port,
                Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                EnableSsl = _smtpConfig.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpConfig.From?.Email!, _smtpConfig.From?.Name),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            attachments.ForEach(x =>
            {
                var attachment = new Attachment(x, "Contract.pdf", MediaTypeNames.Application.Pdf);
                mailMessage.Attachments.Add(attachment);
            });

            foreach (var item in to)
            {
                mailMessage.To.Add(new MailAddress(item.Email, item.Name));
            }

            smtpClient.Send(mailMessage);

            foreach (var attachment in mailMessage.Attachments)
            {
                attachment.Dispose();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendEmailWithAttachmentsAsStreams(SingleEmailToMultipleRecipientsOptions options, List<MemoryStream> attachments)
    {
        await SendWithAttachmentsAsStreams(options.To, options.Subject, options.Body, options.IsBodyHtml, attachments);
    }

    private async Task Send(List<EmailAddressModel> to, string subject, string body, bool isBodyHtml)
    {
        try
        {
            // Using Microsoft Package

            var smtpClient = new SmtpClient(_smtpConfig.Server)
            {
                Port = _smtpConfig.Port,
                Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                EnableSsl = _smtpConfig.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpConfig.From?.Email!, _smtpConfig.From?.Name),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            foreach (var item in to)
            {
                mailMessage.To.Add(new MailAddress(item.Email, item.Name));
            }
            smtpClient.Send(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task SendWithAttachments(List<EmailAddressModel> to, string subject, string body, bool isBodyHtml, List<IFormFile> attachments)
    {
        try
        {
            // Using Microsoft Package

            var smtpClient = new SmtpClient(_smtpConfig.Server)
            {
                Port = _smtpConfig.Port,
                Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                EnableSsl = _smtpConfig.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpConfig.From?.Email!, _smtpConfig.From?.Name),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            attachments.ForEach(x =>
            {
                var attachment = new Attachment(x.OpenReadStream(), x.FileName);
                mailMessage.Attachments.Add(attachment);
            });

            foreach (var item in to)
            {
                mailMessage.To.Add(new MailAddress(item.Email, item.Name));
            }

            smtpClient.Send(mailMessage);

            foreach (var attachment in mailMessage.Attachments)
            {
                attachment.Dispose();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}