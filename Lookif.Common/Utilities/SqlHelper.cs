using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Lookif.Library.Common.Utilities;

public static class SqlHelper
{
    public static DataTable RawSqlQuery(string query, DbContext context, IEnumerable<string> columns)
    {
        using var command = context.Database.GetDbConnection().CreateCommand();

        command.CommandText = query;
        command.CommandType = CommandType.Text;

        context.Database.OpenConnection();

        using (var result = command.ExecuteReader())
        {
            var dataTable = new DataTable();


            dataTable.Load(result);

            return dataTable;
        }


    }






    /// <summary>
    /// Convert Datatabe to IEnumerable<Dictionary<string,string>>
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static IEnumerable<Dictionary<string, string>> ConvertDataTable(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, string>();
            foreach (DataColumn column in row.Table.Columns)
            {
                dict.Add(column.ColumnName, row[column.ColumnName].ToString());

            }
            yield return dict;
        }
    }



    /// <summary>
    /// Convert Datatabe to List<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static List<T> ConvertDataTable<T>(DataTable dt)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }



    private static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(obj, dr[column.ColumnName], null);
                else
                    continue;
            }
        }
        return obj;
    }
}
