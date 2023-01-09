namespace devdeer.tools.tocpm.Helpers
{
	/// <summary>
	/// Provides constant values to the project.
	/// </summary>
	public static class Constants
    {
        #region constants

        /// <summary>
        /// The regular expression to find package references including versions in a csproj file.
        /// </summary>
        public static readonly string OriginalPackageReferenceRegex = "<PackageReference Include=\"(.*)\" Version=\"(.*)\"([ ]?[\\/]?)?>";

        /// <summary>
        /// The regular expression to find any pacakge reference for later deletion in a csproj file.
        /// </summary>
        public static readonly string PackageReferencesToRemoveRegex = "<PackageReference (.*)( Version=\"(.*)\")";

        #endregion
    }
}