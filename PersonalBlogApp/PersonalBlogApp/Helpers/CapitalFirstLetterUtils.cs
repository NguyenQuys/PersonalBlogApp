namespace PersonalBlogApp.Helpers
{
    public static class CapitalFirstLetterUtils
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
