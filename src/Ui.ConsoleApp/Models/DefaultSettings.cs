﻿namespace devdeer.tools.tocpm.Models
{
    using System.ComponentModel;
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
        
        /// <summary>
        /// Indicates if writing the files should be done without any security check.
        /// </summary>
        [CommandOption("-f|--force")]
        [Description("When set the command executes without any security check from the user")]
        public bool? Force { get; set; }
        
        #endregion
    }
}