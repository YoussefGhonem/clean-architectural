namespace TicketManagement.Base.Helpers.Extensions;

public static class StringExtension
{
    // this extension method can be useful in scenarios where you want to handle null strings more gracefully and avoid null reference exceptions
    public static string AsNotNull(this string original)
    {
        return original ?? string.Empty;
    }
}