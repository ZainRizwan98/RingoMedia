namespace RingoMedia.Helper
{
    public static class GenericFunction
    {
        public static DateTime ConvertUtcToTimeZone(DateTime dateTime)
        {
            string timeZoneId = TimeZone.CurrentTimeZone.StandardName;

            // Get the time zone information for the specified time zone
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Convert the given DateTime to the target time zone
            DateTime targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, targetTimeZone);

            return targetDateTime;
        }
    }
}
