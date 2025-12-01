namespace devdeer.tools.tocpm.Helpers
{
    /// <summary>
    /// Provides constant values to the project.
    /// </summary>
    public static class Constants
    {
        #region constants

        /// <summary>
        /// The regular expression to find any package reference for later deletion in a [csproj|fsproj] file.
        /// </summary>
        public static readonly string PackageReferencesToRemoveRegex = "<PackageReference (.*)( Version=\"(.*)\")";

        /// <summary>
        /// The XML body of a Directory.Build.props file including a replace text for the list of packages.
        /// </summary>
        public static readonly string ProjectXmlTemplate =
            "<Project><PropertyGroup><ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally></PropertyGroup><ItemGroup>%PACKAGES%</ItemGroup></Project>";

        #endregion
    }
}