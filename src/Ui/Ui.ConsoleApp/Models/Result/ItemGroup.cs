namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the item group as a holder of package versions for the <see cref="Project" />.
    /// </summary>
    public class ItemGroup
    {
        #region properties

        /// <summary>
        /// The list of package versions.
        /// </summary>
        [XmlElement("PackageVersion")]
        public PackageVersion[] PackageVersions { get; set; } = null!;

        #endregion
    }
}