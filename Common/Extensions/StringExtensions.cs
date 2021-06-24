namespace CustomCommandsSystem.Common.Extensions
{
    internal static class StringExtensions
    {
        public static string RemoveDuplicateSpaces(this string str)
        {
            while (str.Contains("  "))
                str = str.Replace("  ", " ");
            return str;
        }
    }
}
