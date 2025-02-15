﻿using System;
using System.Globalization;

namespace Lookif.Library.Common.Utilities;

public static class StringExtensions
{
    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }
    public static DateTime ToDateTime(this string value)
    {
        return DateTime.Parse(value);
    }
    public static int ToInt(this string value)
    {
        return Convert.ToInt32(value);
    }
    public static int ToInt(this string value, int defaultValue = 0)
    {
        return int.TryParse(value, out int res) ? res : defaultValue;
    }
    public static decimal ToDecimal(this string value)
    {
        value = value.Replace("/", ".");

        return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
    }

    public static string ToNumeric(this int value)
    {
        return value.ToString("N0"); //"123,456"
    }

    public static string ToNumeric(this decimal value)
    {
        return value.ToString("N0");
    }

    public static string ToCurrency(this int value)
    {
        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        return value.ToString("C0");
    }

    public static string ToCurrency(this decimal value)
    {
        return value.ToString("C0");
    }

    public static string En2Fa(this string str)
    {
        return str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
    }
    public static bool IsValidEmail(this string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public static string Fa2En(this string str)
    {
        return str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
    }

    public static string FixPersianChars(this string str)
    {
        return str.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه");//.Replace("ئ", "ی");
    }

    public static string CleanString(this string str)
    {
        return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
    }

    public static string NullIfEmpty(this string str)
    {
        return str?.Length == 0 ? null : str;
    }

    public static string Truncate(this string str, int length = 120)
    {
        return str?.Length > length ? $"{str.Substring(0, length)}..." : str;
    }
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        T result;
        return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
    }

    public static string ToPersianDate(this string date, DateTime defaultValue = default)
    {
        DateTime dt;


        if (!DateTime.TryParse(date, out  dt))
        {
            if (defaultValue == default)
                throw new Exception($"{date} is not a date");


            dt = defaultValue;
 
        }
       

        return dt.ToPersianDate();



    }


    public static DateOnly ToDateOnly(this string date)
    {
        return date.ToDateTime().ToDateOnly(); 
    }
     
}
