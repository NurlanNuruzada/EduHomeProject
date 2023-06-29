namespace HomeEdu.UI.Helpers.Slash
{
        public static class SlashHelper
        {
            public static string ConvertBackslashToSlash(string input)
            {
                return input.Replace("\\", "/");
            }
        }
}
