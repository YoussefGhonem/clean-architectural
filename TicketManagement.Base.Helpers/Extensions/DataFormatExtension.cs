namespace TicketManagement.Base.Helpers.Extensions;

public static class DataFormatExtension
{
    // Load all suffixes in an array  
    private static readonly string[] Suffixes =
        {"Bytes", "KB", "MB", "GB", "TB", "PB"};

    public static string FormatSize(this long bytes)
    {
        // When displaying file sizes in a user interface or file explorer,
        // converting the size in bytes to a more understandable format (e.g., kilobytes, megabytes, etc.) can provide a better user experience.
        if (bytes <= 0) throw new ArgumentOutOfRangeException(nameof(bytes));
        var counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }

        return string.Format("{0:n1}{1}", number, Suffixes[counter]);
    }
}