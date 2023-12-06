namespace devdeer.tools.tocpm.Helpers
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Models;

    /// <summary>
    /// Provides core logic methods.
    /// </summary>
    public static class CoreLogic
    {
        #region methods

        /// <summary>
        /// Tries to detect all csproj files under a certain path.
        /// </summary>
        /// <param name="path">The root path from which to start the search.</param>
        /// <param name="ending">The file ending to search for.</param>
        /// <returns>The list of file information found.</returns>
        public static FileInfo[] CollectProjectFiles(string path, string ending)
        {
            var dirInfo = new DirectoryInfo(path);
            var result = dirInfo.GetFiles($"*.{ending}")
                .ToList();
            foreach (var subDir in dirInfo.GetDirectories())
            {
                result.AddRange(CollectProjectFiles(subDir.FullName, ending));
            }
            return result.ToArray();
        }

        public static string FormatXml(
            this string xml,
            XmlWriterSettings writerSettings,
            XmlReaderSettings? readerSettings = default)
        {
            using (var textReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(textReader, readerSettings))
            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, writerSettings))
                {
                    xmlWriter.WriteNode(xmlReader, true);
                }
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Retrieves the XML content for the prop-file.
        /// </summary>
        /// <param name="packages">The package names with associated versions.</param>
        /// <returns>The XML of the package prop file.</returns>
        public static string GetPackagePropContent(Dictionary<string, PackageInformation> packages)
        {
            var sb = new StringBuilder();
            foreach (var package in packages)
            {
                sb.AppendLine(package.Value.Xml.Replace("PackageReference", "PackageVersion"));
            }
            var test = Constants.ProjectXmlTemplate.Replace("%PACKAGES%", sb.ToString())
                .FormatXml(
                    new XmlWriterSettings
                    {
                        Indent = true,
                        NewLineOnAttributes = false,
                        IndentChars = new string(' ', 4),
                        ConformanceLevel = ConformanceLevel.Auto
                    },
                    new XmlReaderSettings
                    {
                        ConformanceLevel = ConformanceLevel.Auto,
                        IgnoreWhitespace = true
                    });
            return test;
        }

        /// <summary>
        /// Detects all package references in the given <paramref name="files" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assumes that a .NET project file with package references is provided.
        /// </para>
        /// <para>
        /// If multiple packages of the same id are found the one with the highest version is taken.
        /// </para>
        /// </remarks>
        /// <param name="files">The project files to check.</param>
        /// <returns>The list of unique packages.</returns>
        public static Dictionary<string, PackageInformation> GetPackages(FileInfo[] files)
        {
            var result = new Dictionary<string, PackageInformation>();
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file.FullName))
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "PackageReference")
                            {
                                var info = new PackageInformation
                                {
                                    Name = reader.GetAttribute("Include") ?? throw new ApplicationException(
                                        "Invalid csproj version reference. Include is missing."),
                                    Version = reader.GetAttribute("Version") ?? throw new ApplicationException(
                                        "Invalid csproj version reference. Version is missing."),
                                    Xml = reader.ReadOuterXml()
                                };
                                if (!result.ContainsKey(info.Name))
                                {
                                    // just add this package to the results because it wasn't found before
                                    result.Add(info.Name, info);
                                }
                                else
                                {
                                    // replace the package in the results if the version is newer
                                    var currentVersion = result[info.Name].Version;
                                    if (info.Version.IsGreaterThan(currentVersion))
                                    {
                                        result[info.Name] = info;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Removes all versions from <PackageReference /> elements in the given <paramref name="files" />.
        /// </summary>
        /// <param name="files">The list of files in which to remove versions.</param>
        /// <param name="backup">Indicates if every file should be backed up before operation.</param>
        public static void RemoveVersions(FileInfo[] files, bool backup = true)
        {
            var regex = new Regex(Constants.PackageReferencesToRemoveRegex);
            foreach (var file in files)
            {
                if (backup)
                {
                    if (string.IsNullOrEmpty(file.DirectoryName))
                    {
                        throw new ApplicationException(
                            $"Invalid file information for {file} -> directory name is missing.");
                    }
                    file.CopyTo(
                        Path.Combine(file.DirectoryName, file.Name.Replace(file.Extension, $"{file.Extension}.bak")));
                }
                var fileContent = File.ReadAllText(file.FullName);
                var newContent = regex.Replace(fileContent, m => m.Value.Replace(m.Groups[2].Value, ""));
                File.WriteAllText(file.FullName, newContent);
            }
        }

        #endregion
    }
}