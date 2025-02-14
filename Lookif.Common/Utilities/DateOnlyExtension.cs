using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Lookif.Library.Common.Utilities;
public static class DateOnlyExtension
{
    public static DateOnly Today(this DateOnly dt) => DateOnly.FromDateTime(DateTime.Today);
    public static string Today(this DateOnly dt, [StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format) => DateTime.Today.ToString(format);



    public static string ToPersianDate(this DateOnly @do)
    {
        var dt = @do.ToDateTime(default);
        var persianCalendar = new PersianCalendar();
        int persianYear = persianCalendar.GetYear(dt);
        int persianMonth = persianCalendar.GetMonth(dt);
        int persianDay = persianCalendar.GetDayOfMonth(dt);
        return $"{persianYear:00}{persianMonth:00}{persianDay:00}";

    }

}
