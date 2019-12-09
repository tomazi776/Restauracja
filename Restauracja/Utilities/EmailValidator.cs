using System.Text.RegularExpressions;

namespace Restauracja.Utilities
{
    public static class EmailValidator
    {
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
    }
}
