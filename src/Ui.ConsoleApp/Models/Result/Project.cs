namespace devdeer.tools.tocpm.Models.Result
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Project
    {
        #region methods

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

        public string ToXmlText()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                var serializer = new XmlSerializer(GetType());
                serializer.Serialize(writer, this, ns);
                return stream.ToString();
            }
        }

        #endregion

        #region properties

        public PropertyGroup PropertyGroup { get; set; }

        public ItemGroup ItemGroup { get; set; }

        #endregion
    }
}