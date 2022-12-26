namespace devdeer.tools.tocpm.Helpers
{
    using System.Text.RegularExpressions;

    public static class MarkupHelper
    {
        #region methods

        /// <summary>
        /// Takes an XML as the <paramref name="input" /> and adds nice console markup to the elements.
        /// </summary>
        /// <param name="input">The XML input string.</param>
        /// <returns>The formatted string for the console.</returns>
        public static string AddConsoleMarkup(string input)
        {
            var result = Regex.Replace(input, @"<([\/]?(.*))[ |>]", @"[deepskyblue4]$0[/]");
            result = Regex.Replace(result, "\"([\\w|.]*)\"", @"[green4]$0[/]");
            result = Regex.Replace(result, ">(.*)<", @">[deeppink4]$1[/]<");
            return result;
        }

        #endregion
    }
}