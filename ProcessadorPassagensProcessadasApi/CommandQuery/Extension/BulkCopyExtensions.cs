using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Extension
{
    public static class BulkCopyExtensions
    {

        public static void BulkInsert<T>(this IDbConnection conn,
            IList<T> dataList,
            string destinationTableName,
            Func<long, int> sqlRowsCopied = null)
        {
            using (var sqlDestination = new SqlConnection(conn.ConnectionString))
            {
                if (sqlDestination.State != ConnectionState.Open)
                    sqlDestination.Open();

                using (var sqlBulkCopy = new SqlBulkCopy(sqlDestination))
                {
                    sqlBulkCopy.BatchSize = dataList.Count;
                    sqlBulkCopy.DestinationTableName = destinationTableName;
                    var dataTable = dataList.ToDataTable();
                    sqlBulkCopy.WriteToServer(dataTable);
                }
            }

        }


        
    }
}
