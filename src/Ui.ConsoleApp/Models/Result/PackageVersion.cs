namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml.Serialization;

    public class PackageVersion
    {
        #region constructors and destructors


        #endregion

        #region properties

        [XmlAttribute]
        public string Include { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        #endregion
    }
}