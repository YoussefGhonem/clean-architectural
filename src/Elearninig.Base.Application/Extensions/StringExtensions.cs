namespace Elearninig.Base.Application.Extensions
{
    public static class StringExtensions
    {
        // ParseEnum method is used to convert the string value to the specified enumeration type T
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
