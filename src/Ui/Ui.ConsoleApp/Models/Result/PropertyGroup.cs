namespace devdeer.tools.tocpm.Models.Result
{
    /// <summary>
    /// Represents the top property group with the settings for the <see cref="Project" />.
    /// </summary>
    public class PropertyGroup
    {
        #region properties

        /// <summary>
        /// Indicates if CPM should be used.
        /// </summary>
        public bool ManagePackageVersionsCentrally { get; set; }

        #endregion
    }
}