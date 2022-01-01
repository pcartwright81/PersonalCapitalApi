namespace PersonalCapital.ConsoleApp.Extensions
{
    public static class StringExtensions
    {
        public static string WhenNullOrEmpty(this string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}