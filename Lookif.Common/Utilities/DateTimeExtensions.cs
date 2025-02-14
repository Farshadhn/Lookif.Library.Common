using System;
using System.Globalization;

namespace Lookif.Library.Common.Utilities;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Saturday)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
    public static DateTime StartOfMonth(this DateTime dt, bool persian = true)
    {
        var pNow = new PersianCalendar();
        return (persian) ? dt.AddDays(-1 * pNow.GetDayOfMonth(dt) + 1) : new DateTime(dt.Year, dt.Month, 1);
    }

    public static DateTime StartOfYear(this DateTime dt, bool persian = true)
    {
        var pNow = new PersianCalendar();
        return (persian) ? dt.AddMonths(-1 * pNow.GetMonth(dt) + 1).StartOfMonth() : new DateTime(dt.Year, 1, 1);

    }


    public static string ToPersianDate(this DateTime dt, bool persian = true)
    {
        var persianCalendar = new PersianCalendar();
        int persianYear = persianCalendar.GetYear(dt);
        int persianMonth = persianCalendar.GetMonth(dt);
        int persianDay = persianCalendar.GetDayOfMonth(dt);
        return $"{persianYear:00}{persianMonth:00}{persianDay:00}";

    }



    public static long ToMillisecondsTimestamp(this DateTime date)
    {
        DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(date.ToUniversalTime() - baseDate).TotalMilliseconds;
    }

    public static DateOnly ToDateOnly(this DateTime date)
    {
        return DateOnly.FromDateTime(date);
    }

}
