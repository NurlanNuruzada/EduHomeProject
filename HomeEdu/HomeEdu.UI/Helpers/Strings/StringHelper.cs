namespace HomeEdu.UI.Helpers.Strings
{
    public static class StringHelper
    {
        public static string TruncateString(string input, int maxLength)
        {
            if (input.Length <= maxLength)
            {
                return input;
            }
            else
            {
                return input.Substring(0, maxLength);
            }
        }
    }
}
