namespace devdeer.tools.tocpm.Helpers
{
    using Models;

    using Spectre.Console;

    /// <summary>
    /// Provides helper methods for output operations.
    /// </summary>
    public static class OutputHelper
    {
        #region methods

        /// <summary>
        /// Prints the <paramref name="packages" /> as a table in a sorted manner.
        /// </summary>
        /// <param name="packages">The dictionary of detected packages.</param>
        public static void PrintPackages(Dictionary<string, PackageInformation> packages)
        {
            var table = new Table();
            table.Border(TableBorder.Square);
            table.AddColumn(new TableColumn(new Markup("Package ID")));
            table.AddColumn(new TableColumn("Version"));
            foreach (var package in packages.Keys.OrderBy(k => k))
            {
                var version = Markup.Escape(packages[package].Version);
                table.AddRow(package, version);
            }
            AnsiConsole.Write(table);
        }

        #endregion
    }
}