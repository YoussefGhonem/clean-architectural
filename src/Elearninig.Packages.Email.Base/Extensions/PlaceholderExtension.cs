using Elearninig.Packages.Email.Base.Models;

namespace Elearninig.Packages.Email.Base.Extensions;

public static class PlaceholderExtension
{
    public static string ReplacePlaceholders(this string htmlBody, List<TemplatePlaceholderModel> placeholders)
    {
        foreach (var item in placeholders)
        {
            var oldValue = item.Placeholder!.Trim();
            htmlBody = htmlBody.Replace(oldValue, item.Value);
        }

        return htmlBody;
    }
}