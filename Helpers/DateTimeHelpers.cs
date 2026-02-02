namespace Psxbox.Utils
{
    public static class DateTimeHelpers
    {

        public static DateTime StartOfAHour(this DateTime dateTime) =>
            new(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, millisecond: 0);
        public static DateTime EndOfAHour(this DateTime dateTime) =>
            new(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 59, 59, millisecond: 999);
        public static DateTime StartOfADay(this DateTime dateTime) =>
            new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, millisecond: 0);
        public static DateTime EndOfADay(this DateTime dateTime) =>
            new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, millisecond: 999);
        public static DateTime StartOfAMonth(this DateTime dateTime) =>
            new(dateTime.Year, dateTime.Month, 1, 0, 0, 0, millisecond: 0);
        public static DateTime EndOfAMonth(this DateTime dateTime) =>
            dateTime.StartOfAMonth().AddMonths(1).AddMilliseconds(-1);
        public static DateTime StartOfAYear(this DateTime dateTime) =>
           new(dateTime.Year, 1, 1, 0, 0, 0, millisecond: 0);
        public static DateTime EndOfAYear(this DateTime dateTime) =>
            new(dateTime.Year, 12, 31, 23, 59, 59, millisecond: 999);

        public static DateTime AddPeriod(this DateTime dateTime, string period)
        {
            int number = int.Parse(period[..^1]);
            char unit = period[^1];

            return unit switch
            {
                's' => dateTime.AddSeconds(number),
                'm' => dateTime.AddMinutes(number),
                'h' => dateTime.AddHours(number),
                'd' => dateTime.AddDays(number),
                'M' => dateTime.AddMonths(number),
                'y' => dateTime.AddYears(number),
                _ => throw new ArgumentException("Invalid period unit"),
            };
        }

        public static DateTime AddPeriod(this DateTime dateTime, Period period, int value)
        {
            return period switch
            {
                Period.Milliseconds => dateTime.AddMilliseconds(value),
                Period.Seconds => dateTime.AddSeconds(value),
                Period.Minutes => dateTime.AddMinutes(value),
                Period.Hours => dateTime.AddHours(value),
                Period.Days => dateTime.AddDays(value),
                Period.Months => dateTime.AddMonths(value),
                Period.Years => dateTime.AddYears(value),
                _ => throw new ArgumentException("Invalid period unit"),
            };
        }

        public static string HumanizeDate(this DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;

            if (ts < TimeSpan.FromMinutes(1))
                return ts.Seconds == 0 ? "Hozir" : $"{ts.Seconds} sekund oldin";
            else if (ts < TimeSpan.FromHours(1))
                return $"{ts.Minutes} minut oldin";
            else if (ts < TimeSpan.FromDays(1))
                return $"{ts.Hours} soat " + (ts.Minutes > 0 ? $"{ts.Minutes} minut " : "") + "oldin";
            else if (ts >= TimeSpan.FromDays(1))
                return $"{ts.Days} kun " + (ts.Hours > 0 ? $"{ts.Hours} soat " : "") + "oldin";
            else return dateTime.ToString("G");
        }

        public static string HumanizeDate(this DateTimeOffset dateTime) => dateTime.LocalDateTime.HumanizeDate();

        public static int GetTotalMonths(DateTime startDate, DateTime endDate)
        {
            // Ensure that startDate is earlier than endDate
            if (startDate > endDate)
            {
                throw new ArgumentException("startDate should be earlier than endDate");
            }

            int totalMonths = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

            // Adjust for partial months
            if (endDate.Day < startDate.Day)
            {
                totalMonths--;
            }

            return totalMonths;
        }

        public static int GetTotalMonths(DateTimeOffset startDate, DateTimeOffset endDate) => GetTotalMonths(startDate.LocalDateTime, endDate.LocalDateTime);
    }
}
