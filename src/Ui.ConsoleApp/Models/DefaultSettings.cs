namespace devdeer.tools.tocpm.Models
{
    using System.ComponentModel;

    using Spectre.Console.Cli;

    /// <summary>
    /// The default settings for passing in information from the command line.
    /// </summary>
    public class DefaultSettings : CommandSettings
    {
        #region properties

        /// <summary>
        /// The path where to run the tool in.
        /// </summary>
        [CommandArgument(0, "[PATH]")]
        public string Path { get; set; } = null!;

        /// <summary>
        /// Indicates if writing the files should be done without any security check.
        /// </summary>
        [CommandOption("-f|--force")]
        [Description("If set, the command executes without any security check from the user")]
        public bool? Force { get; set; }

        /// <summary>
        /// Indicates if writing the files should be done without any security check.
        /// </summary>
        [CommandOption("-b|--backup")]
        [Description("If set, the command will generate a backup of every project file before editing it.")]
        public bool? Backup { get; set; }

        #endregion
    }
}