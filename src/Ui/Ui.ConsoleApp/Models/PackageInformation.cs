namespace devdeer.tools.tocpm.Models
{
    /// <summary>
    /// Represents the information about a single Nuget package from a csproj or fsproj file.
    /// </summary>
    public class PackageInformation
    {
        #region properties

        /// <summary>
        /// The package name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// The version string.
        /// </summary>
        public string Version { get; set; } = default!;

        /// <summary>
        /// The complete XML tag of the property.
        /// </summary>
        public string Xml { get; set; } = default!;

        #endregion
    }
}