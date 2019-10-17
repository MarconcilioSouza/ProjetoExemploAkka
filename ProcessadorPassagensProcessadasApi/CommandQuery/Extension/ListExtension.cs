using System.Collections.Generic;
using System.Data;
using FastMember;
using System;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Extension
{
    public static class ListExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var propriedades = typeof(T).GetProperties().Select(c => c.Name).ToArray();
            DataTable dataTable = new DataTable();

            var listNotNull = new List<T>();
            foreach (var item in list)
            {
                if (item != null)
                    listNotNull.Add(item);
            }

            using (var reader = ObjectReader.Create(listNotNull, propriedades))
            {
                dataTable.Load(reader);
            }

            return dataTable;
        }
    }
}
