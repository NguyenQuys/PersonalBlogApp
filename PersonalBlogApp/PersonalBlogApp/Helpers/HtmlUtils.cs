using System.Text.RegularExpressions;

namespace PersonalBlogApp.Helpers
{
    public static class HtmlUtils
    {
        public static string StripHtml(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
