namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a single entry in the <see cref="ItemGroup" />.
    /// </summary>
    public class PackageVersion
    {
        #region properties

        /// <summary>
        /// The ID of the package.
        /// </summary>
        [XmlAttribute]
        public string Include { get; set; } = null!;

        /// <summary>
        /// The version of the package.
        /// </summary>
        [XmlAttribute]
        public string Version { get; set; } = null!;

        #endregion
    }
}