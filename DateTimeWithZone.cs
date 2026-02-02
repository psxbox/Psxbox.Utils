namespace Psxbox.Utils;

public struct DateTimeWithZone
{
    private readonly DateTime utcDateTime;
    private readonly TimeZoneInfo timeZone;

    public static TimeZoneInfo UzbekistanTimeZoneInfo =>
           TimeZoneInfo.CreateCustomTimeZone("Uzbekistan", TimeSpan.FromHours(5),
                   "(UTC+05:00) Uzbekistan time", "Uzbekistan time");

    public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
    {
        var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
        this.timeZone = timeZone;
    }

    public DateTime UniversalTime => utcDateTime;

    public TimeZoneInfo TimeZone => timeZone;

    public DateTime LocalTime => TimeZoneInfo.ConvertTime(utcDateTime, timeZone);
}
