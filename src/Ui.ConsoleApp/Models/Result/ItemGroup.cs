namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml.Serialization;

    public class ItemGroup
    {
        #region constructors and destructors

        #endregion

        #region properties

        [XmlElement("PackageVersion")]
        public PackageVersion[] PackageVersions { get; set; }

        #endregion
    }
}