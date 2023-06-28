namespace TicketManagement.Base.Helpers.Extensions;

public static class ReferenceGenerator
{
    // this methode used in entity to Generate the Reference field
    public static string Generator(string code)
    {
        Random rd = new();
        return code + DateTime.Now.ToString("yyyyMMddfff" + rd.Next(0, 9) + rd.Next(0, 9));
    }
}