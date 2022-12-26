namespace devdeer.tools.tocpm.Helpers
{
    using System.Text.RegularExpressions;

    using Models.Result;

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

        /// <summary>
        /// Retrieves the XML content for the prop-file.
        /// </summary>
        /// <param name="packages">The package names with associated versions.</param>
        /// <returns>The XML of the package prop file.</returns>
        public static string GetPackagePropContent(Dictionary<string, string> packages)
        {
            var result = Project.FromDictionary(packages);
            return result.ToXmlText();
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
        public static Dictionary<string, string> GetPackages(FileInfo[] files)
        {
            var result = new Dictionary<string, string>();
            var regex = new Regex("<PackageReference Include=\"(.*)\" Version=\"(.*)\" />");
            foreach (var file in files)
            {
                var matches = regex.Matches(File.ReadAllText(file.FullName));
                foreach (var match in matches.Cast<Match>())
                {
                    var name = match.Groups[1]
                        .Value;
                    var version = match.Groups[2]
                        .Value;
                    if (!result.ContainsKey(name))
                    {
                        result.Add(name, version);
                    }
                    else
                    {
                        var currentVersion = result[name];
                        if (version.IsBiggerThan(currentVersion))
                        {
                            result[name] = version;
                        }
                    }
                }
            }
            return result;
        }

        #endregion
    }
}