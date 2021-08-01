namespace Common.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string ToKvp(this string key, object value)
        {
            return $"{key}={value}";
        }
    }
}
