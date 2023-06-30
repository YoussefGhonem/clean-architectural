using System.Text;

namespace Elearninig.Packages.Storage.AzureBlob.Extensions;
public static class BlobContainerNameValidator
{
    public static string EditeContainerName(this string containerName)
    {
        containerName = RemoveSpecialCharacters(containerName.Trim().ToLower());
        CheckDoubleDashCharacters(containerName);
        CheckBeginEndCharacters(containerName);
        CheckContainerNameLength(containerName);
        return containerName;
    }
    private static string RemoveSpecialCharacters(string str)
    {
        var sb = new StringBuilder();
        foreach (char c in str)
        {
            if (c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c == '-')
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
    private static void CheckDoubleDashCharacters(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '-' && str[i + 1] == '-')
            {
                throw new Exception(
                    "container name can't contain two (--) without separate them by integer or number");
            }
        }
    }
    private static void CheckBeginEndCharacters(string str)
    {
        if (!(str[0] >= '0' && str[0] <= '9') && !(str[0] >= 'a' && str[0] <= 'z'))
            throw new Exception("container name can't begin without number or character.");


        if (!(str[str.Length - 1] >= '0' && str[str.Length - 1] <= '9') &&
            !(str[str.Length - 1] >= 'a' && str[str.Length - 1] <= 'z'))
            throw new Exception("container name can't end without number or character.");
    }
    private static void CheckContainerNameLength(string str)
    {
        if (str.Length < 3 || str.Length > 63)
            throw new Exception("container name must be from 3 through 63 characters long.");
    }
}
