using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace Elearninig.Packages.Email.Base.Extensions;

public static class HtmlTemplateExtension
{
    public static string ExtractStringFromHtml(this IHostingEnvironment hostingEnvironment, string templateName)
    {
        var templatePath = GetTemplatePath(templateName, hostingEnvironment);
        var builder = new BodyBuilder();
        using (StreamReader SourceReader = File.OpenText(templatePath))
        {
            builder.HtmlBody = SourceReader.ReadToEnd();
        }

        return builder.HtmlBody;
    }

    private static string GetTemplatePath(string templateName, IHostingEnvironment hostingEnvironment)
    {
        var pathToFile = hostingEnvironment.WebRootPath
                         + Path.DirectorySeparatorChar.ToString()
                         + "EmailTemplates"
                         + Path.DirectorySeparatorChar.ToString()
                         + templateName;
        return pathToFile;
    }
}