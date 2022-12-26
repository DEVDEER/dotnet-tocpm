namespace devdeer.tools.tocpm.Commands
{
    using System.Text.RegularExpressions;

    using Helpers;

    using Models;
    using Models.Result;

    using Spectre.Console;
    using Spectre.Console.Cli;

    /// <summary>
    /// Abstract base class for the commands.
    /// </summary>
    public abstract class BaseCommand : Command<DefaultSettings>
    {
        #region constants

        private const string CpmFileName = "Directory.Packages.props";

        #endregion

        #region methods

        /// <inheritdoc />
        public override int Execute(CommandContext context, DefaultSettings settings)
        {
            var path = settings.Path;
            if (!Directory.Exists(path))
            {
                var ex = new DirectoryNotFoundException();
                AnsiConsole.WriteException(ex);
                return -1;
            }
            var targetFile = Path.Combine(path, CpmFileName);
            var success = false;
            string? markupResult = null;
            AnsiConsole.MarkupLine("Running...");
            AnsiConsole.Status()
                .Start(
                    "Collecting files...",
                    ctx =>
                    {
                        // collect files
                        ctx.Spinner(Spinner.Known.Default);
                        var files = CollectProjectFiles(path, "csproj");
                        AnsiConsole.MarkupLine($"Found [bold yellow]{files.Length}[/] files.");
                        if (files.Length == 0)
                        {
                            // nothing to do
                            return;
                        }
                        ctx.Status = "Reading packages...";
                        var packages = GetPackages(files);
                        AnsiConsole.MarkupLine($"Found [bold yellow]{packages.Count}[/] unique packages.");
                        if (!packages.Any())
                        {
                            return;
                        }
                        PrintPackages(packages);
                        ctx.Status = "Generating CPM file...";
                        var result = GetPackagePropContent(packages);
                        if (OnlySimulate)
                        {
                            markupResult = AddConsoleMarkup(result);
                            success = true;
                            return;
                        }
                        File.WriteAllText(targetFile, result);
                        AnsiConsole.MarkupLine($"File [bold white]{targetFile}[/] was generated.");
                        ctx.Status = "Removing explicit versions in project files...";
                        // TODO remove versions in csproj
                        success = true;
                    });
            if (success && !OnlySimulate && !string.IsNullOrEmpty(markupResult))
            {
                AnsiConsole.MarkupLine($"The following content would be written to [bold white]{targetFile}[/] if executed with command [bold white]execute[/]:");
                AnsiConsole.Markup(markupResult);
            }
            return success ? 0 : 1;
        }

        /// <summary>
        /// Takes an XML as the <paramref name="input" /> and adds nice console markup to the elements.
        /// </summary>
        /// <param name="input">The XML input string.</param>
        /// <returns>The formatted string for the console.</returns>
        private string AddConsoleMarkup(string input)
        {
            var result = Regex.Replace(input, @"<([\/]?(.*))[ |>]", @"[deepskyblue4]$0[/]");
            result = Regex.Replace(result, "\"([\\w|.]*)\"", @"[green4]$0[/]");
            result = Regex.Replace(result, ">(.*)<", @">[deeppink4]$1[/]<");
            return result;
        }

        /// <summary>
        /// Tries to detect all csproj files under a certain path.
        /// </summary>
        /// <param name="path">The root path from which to start the search.</param>
        /// <param name="ending">The file ending to search for.</param>
        /// <returns>The list of file information found.</returns>
        private FileInfo[] CollectProjectFiles(string path, string ending)
        {
            var dirInfo = new DirectoryInfo(path);
            var result = dirInfo.GetFiles($"*.{ending}")
                .ToList();
            foreach (var subDir in dirInfo.GetDirectories())
            {
                result.AddRange(CollectProjectFiles(subDir.FullName, ending));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Retrieves the XML content for the prop-file.
        /// </summary>
        /// <param name="packages">The package names with associated versions.</param>
        /// <returns>The XML of the package prop file.</returns>
        private string GetPackagePropContent(Dictionary<string, string> packages)
        {
            var result = Project.FromDictionary(packages);
            return result.ToXmlText();
        }

        /// <summary>
        /// Detects all package references in the given <paramref name="files" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assumes that a .NET project file with package references is provided.
        /// </para>
        /// <para>
        /// If multiple packages of the same id are found the one with the highest version is taken.
        /// </para>
        /// </remarks>
        /// <param name="files">The project files to check.</param>
        /// <returns>The list of unique packages.</returns>
        private Dictionary<string, string> GetPackages(FileInfo[] files)
        {
            var result = new Dictionary<string, string>();
            var regex = new Regex("<PackageReference Include=\"(.*)\" Version=\"(.*)\" />");
            foreach (var file in files)
            {
                var matches = regex.Matches(File.ReadAllText(file.FullName));
                foreach (var match in matches.Cast<Match>())
                {
                    var name = match.Groups[1]
                        .Value;
                    var version = match.Groups[2]
                        .Value;
                    if (!result.ContainsKey(name))
                    {
                        result.Add(name, version);
                    }
                    else
                    {
                        var currentVersion = result[name];
                        if (version.IsBiggerThan(currentVersion))
                        {
                            result[name] = version;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Prints the <paramref name="packages" /> as a table in a sorted manner.
        /// </summary>
        /// <param name="packages">The dictionary of detected packages.</param>
        private void PrintPackages(Dictionary<string, string> packages)
        {
            var table = new Table();
            table.Border(TableBorder.Square);
            table.AddColumn(new TableColumn(new Markup("[yellow]Package ID[/]")));
            table.AddColumn(new TableColumn("[blue]Version[/]"));
            foreach (var package in packages.Keys.OrderBy(k => k))
            {
                table.AddRow(package, packages[package]);
            }
            AnsiConsole.Write(table);
        }

        #endregion

        #region properties

        /// <summary>
        /// Must be overridden by children to indicate if only a simulated run should be performed.
        /// </summary>
        public abstract bool OnlySimulate { get; }

        #endregion
    }
}