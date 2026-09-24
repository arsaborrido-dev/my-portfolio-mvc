using System.Text.RegularExpressions;

namespace my_portfolio_mvc.Security
{
    public static class InputSanitizer
    {
        public static string Sanitize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Very simple example: strip HTML tags
            var withoutTags = Regex.Replace(input, "<.*?>", string.Empty);

            // Encode any remaining special chars to avoid XSS
            return System.Net.WebUtility.HtmlEncode(withoutTags);
        }
    }
}
