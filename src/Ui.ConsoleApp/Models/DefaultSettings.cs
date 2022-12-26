namespace devdeer.tools.tocpm.Models
{
    using System.Reflection;

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
        public string Path { get; set; }
        
        #endregion
    }
}