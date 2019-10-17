using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Extension
{
    public static class TryParseExtensions
    {

        public static bool Between(this DateTime checker, DateTime floor, DateTime ceiling)
        {
            return checker <= ceiling && checker >= floor;
        }
        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        public static int TryToInt(this object valor)
        {
            var v = valor?.ToString() ?? "";
            int.TryParse(v, out int i);
            return i;
        }

        public static decimal TryToDecimal(this object valor)
        {
            var v = valor?.ToString() ?? "";
            decimal.TryParse(v, out decimal i);
            return i;
        }

        public static long TryToLong(this object valor)
        {
            var v = valor?.ToString() ?? "";
            long.TryParse(v, out long i);
            return i;
        }

        public static DateTime? TryToDateTime(this object valor)
        {
            if (valor == null)
                return null;

            var v = valor?.ToString() ?? "";
            DateTime.TryParse(v, out DateTime date);
            return date;
        }

        public static string TryToString(this object valor)
        {
            return valor?.ToString() ?? "";
        }

        public static DateTime FromUnixDate(this long unixDate)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixDate);
        }

        public static bool ToBool(this int valor)
        {
            return valor == 1;
        }

        public static DateTime AddWorkingDays(this DateTime dateVal, double value, List<DateTime> Feriados = null)
        {
            var returnValue = dateVal;

            if (value == 0)
            {
                returnValue = dateVal.AddDays(-1);
                value = 1;
            }

            for (var i = 0; i < value; i++)
            {
                returnValue = returnValue.AddDays(1);

                while (!returnValue.IsWorkingDay(Feriados))
                {
                    returnValue = returnValue.AddDays(1);
                }
            }

            return returnValue;
        }

        public static bool IsWorkingDay(this DateTime dateVal, List<DateTime> Feriados = null)
        {
            var isWorkingDay = dateVal.DayOfWeek != DayOfWeek.Sunday && dateVal.DayOfWeek != DayOfWeek.Saturday;

            if (isWorkingDay && Feriados != null && Feriados.Contains(dateVal))
                return false;

            return isWorkingDay;
        }

        public static long ToUnixDate(this DateTime dateTime)
        {
            return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute?.Description ?? "";
        }

    }
}
