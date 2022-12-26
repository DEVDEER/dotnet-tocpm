namespace devdeer.tools.tocpm.Helpers
{
    public static class VersionHelper
    {
        #region methods

        public static bool IsBiggerThan(this string version, string otherVersion)
        {
            return version.ToNumericValue() > otherVersion.ToNumericValue();
        }

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