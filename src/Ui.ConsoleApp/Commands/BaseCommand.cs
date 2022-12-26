namespace devdeer.tools.tocpm.Commands
{
    using Helpers;

    using Models;

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
                        var files = CoreLogic.CollectProjectFiles(path, "csproj");
                        AnsiConsole.MarkupLine($"Found [bold yellow]{files.Length}[/] files.");
                        if (files.Length == 0)
                        {
                            // nothing to do
                            return;
                        }
                        ctx.Status = "Reading packages...";
                        var packages = CoreLogic.GetPackages(files);
                        AnsiConsole.MarkupLine($"Found [bold yellow]{packages.Count}[/] unique packages.");
                        if (!packages.Any())
                        {
                            return;
                        }
                        OutputHelper.PrintPackages(packages);
                        ctx.Status = "Generating CPM file...";
                        var result = CoreLogic.GetPackagePropContent(packages);
                        if (OnlySimulate)
                        {
                            markupResult = MarkupHelper.AddConsoleMarkup(result);
                            success = true;
                            return;
                        }
                        File.WriteAllText(targetFile, result);
                        AnsiConsole.MarkupLine($"File [bold white]{targetFile}[/] was generated.");
                        ctx.Status = "Removing explicit versions in project files...";
                        // TODO remove versions in csproj
                        success = true;
                    });
            if (success && OnlySimulate && !string.IsNullOrEmpty(markupResult))
            {
                AnsiConsole.MarkupLine($"The following content would be written to [bold white]{targetFile}[/] if executed with command [bold white]execute[/]:");
                AnsiConsole.Markup(markupResult);
            }
            return success ? 0 : 1;
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