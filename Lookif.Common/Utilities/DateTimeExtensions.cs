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
}
