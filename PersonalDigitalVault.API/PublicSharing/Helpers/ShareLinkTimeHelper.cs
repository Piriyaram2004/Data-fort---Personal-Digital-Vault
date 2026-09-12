namespace PersonalDigitalVault.API.PublicSharing.Helpers
{
    public static class ShareLinkTimeHelper
    {
        private static readonly TimeZoneInfo SriLankaTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows()
                    ? "Sri Lanka Standard Time"
                    : "Asia/Colombo");

        public static DateTime ConvertLocalToUtc(DateTime localDateTime)
        {
            var unspecifiedDateTime =
                DateTime.SpecifyKind(
                    localDateTime,
                    DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(
                unspecifiedDateTime,
                SriLankaTimeZone);
        }

        public static bool IsExpired(DateTime? expiresAtUtc)
        {
            return expiresAtUtc.HasValue &&
                   expiresAtUtc.Value <= DateTime.UtcNow;
        }
    }
}