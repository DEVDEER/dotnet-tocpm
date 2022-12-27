namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the resulting XML root node.
    /// </summary>
    public class Project
    {
        #region methods

        /// <summary>
        /// Factory method to generate an instance of this type using the information in the <paramref name="packages" />.
        /// </summary>
        /// <param name="packages">The dictionary of package ids and versions.</param>
        /// <returns>The constructed instance.</returns>
        public static Project FromDictionary(Dictionary<string, string> packages)
        {
            return new Project
            {
                PropertyGroup = new PropertyGroup
                {
                    ManagePackageVersionsCentrally = true
                },
                ItemGroup = new ItemGroup
                {
                    PackageVersions = packages.Keys.OrderBy(k => k)
                        .Select(
                            k => new PackageVersion
                            {
                                Include = k,
                                Version = packages[k]
                            })
                        .ToArray()
                }
            };
        }

        /// <summary>
        /// Retrieves this instance serialized as XML.
        /// </summary>
        /// <returns>The serialized XML text.</returns>
        public string ToXmlText()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            using var stream = new StringWriter();
            using var writer = XmlWriter.Create(stream, settings);
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this, ns);
            return stream.ToString();
        }

        #endregion

        #region properties

        /// <summary>
        /// The head property group.
        /// </summary>
        public PropertyGroup PropertyGroup { get; set; } = null!;

        /// <summary>
        /// The item group.
        /// </summary>
        public ItemGroup ItemGroup { get; set; } = null!;

        #endregion
    }
}