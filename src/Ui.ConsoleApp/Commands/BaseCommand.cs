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
            string? result = null;
            string? markupResult = null;
            FileInfo[]? files = null;
            AnsiConsole.MarkupLine("Running...");
            AnsiConsole.Status()
                .Start(
                    "Collecting files...",
                    ctx =>
                    {
                        // collect files
                        ctx.Spinner(Spinner.Known.Default);
                        files = CoreLogic.CollectProjectFiles(path, "csproj");
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
                        result = CoreLogic.GetPackagePropContent(packages);
                        if (OnlySimulate)
                        {
                            markupResult = MarkupHelper.AddConsoleMarkup(result);
                            success = true;
                        }
                    });
            if (success && OnlySimulate && !string.IsNullOrEmpty(markupResult))
            {
                AnsiConsole.MarkupLine($"The following content would be written to [bold white]{targetFile}[/] if executed with command [bold white]execute[/]:");
                AnsiConsole.Markup(markupResult);
                // simulation completed
                return 0;
            }
            if (!settings.Force ?? true)
            {
                // ge
                if (!AnsiConsole.Confirm("Do you want to execute the replacement now?"))
                {
                    AnsiConsole.MarkupLine("Operation cancelled by user.");
                    return 1;
                }
            }
            AnsiConsole.Status()
                .Start(
                    "Tranforming folder to CPM...",
                    ctx =>
                    {
                        try
                        {
                            File.WriteAllText(targetFile, result);
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.WriteException(ex);
                            return;
                        }
                        AnsiConsole.MarkupLine($"File [bold white]{targetFile}[/] was generated.");
                        ctx.Status = "Removing explicit versions in project files...";
                        try
                        {
                            CoreLogic.RemoveVersions(files, settings.Backup ?? false);
                            AnsiConsole.MarkupLine($"Command succeeded. [bold yellow]{files.Length}[/] files where edited and [bold yellow]{targetFile}[/] was generated in working folder.");
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.WriteException(ex);
                        }
                    });
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