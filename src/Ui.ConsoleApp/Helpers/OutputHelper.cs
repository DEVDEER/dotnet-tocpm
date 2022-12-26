namespace devdeer.tools.tocpm.Helpers
{
    using System.Text.RegularExpressions;

    using Spectre.Console;

    public static class OutputHelper
    {
        #region methods

        

        /// <summary>
        /// Prints the <paramref name="packages" /> as a table in a sorted manner.
        /// </summary>
        /// <param name="packages">The dictionary of detected packages.</param>
        public static void PrintPackages(Dictionary<string, string> packages)
        {
            var table = new Table();
            table.Border(TableBorder.Square);
            table.AddColumn(new TableColumn(new Markup("Package ID")));
            table.AddColumn(new TableColumn("Version"));
            foreach (var package in packages.Keys.OrderBy(k => k))
            {
                table.AddRow(package, packages[package]);
            }
            AnsiConsole.Write(table);
        }

        #endregion
    }
}