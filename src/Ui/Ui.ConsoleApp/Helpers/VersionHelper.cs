namespace devdeer.tools.tocpm.Helpers
{
    /// <summary>
    /// Provides helper methods for version strings.
    /// </summary>
    public static class VersionHelper
    {
        #region methods

        /// <summary>
        /// Decides if the given <paramref name="version" /> is numerically greater than the <paramref name="otherVersion" />.
        /// </summary>
        /// <param name="version">The version for which to perform the comparison.</param>
        /// <param name="otherVersion">The version with which to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="version" /> is greater than <paramref name="otherVersion" />, otherwise
        /// <c>false</c>.
        /// </returns>
        public static bool IsGreaterThan(this string version, string otherVersion)
        {
            return version.ToNumericValue() > otherVersion.ToNumericValue();
        }

        /// <summary>
        /// Generates a numeric value from the given <paramref name="version" />.
        /// </summary>
        /// <param name="version">The version as a string.</param>
        /// <returns>The numeric representation.</returns>
        public static int ToNumericValue(this string version)
        {
            var result = 0;
            var parts = version.Split(".");
            var factor = 10000;
            foreach (var part in parts)
            {
                if (!int.TryParse(part, out var numericPart))
                {
                    break;
                }
                result += numericPart * factor;
                factor /= 10;
            }
            return result;
        }

        #endregion
    }
}