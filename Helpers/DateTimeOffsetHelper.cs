namespace Psxbox.Utils.Helpers;

public static class DateTimeOffsetHelper
{
    public static DateTimeOffset StartOfDay(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Offset);
    }

    public static DateTimeOffset EndOfDay(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Offset);
    }

    public static DateTimeOffset StartOfMonth(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Offset);
    }

    public static DateTimeOffset EndOfMonth(this DateTimeOffset dateTime)
    {
        return dateTime.StartOfMonth().AddMonths(1).AddMilliseconds(-1);
    }

    public static DateTimeOffset StartOfYear(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, 1, 1, 0, 0, 0, dateTime.Offset);
    }

    public static DateTimeOffset EndOfYear(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, 12, 31, 23, 59, 59, 999, dateTime.Offset);
    }
}