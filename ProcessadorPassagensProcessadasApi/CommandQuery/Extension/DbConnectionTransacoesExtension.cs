using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Extension
{
    public static class DbConnectionTransacoesExtension
    {
        /// <summary>
        /// Realiza o bulk insert de uma lista de dados.
        /// </summary>
        /// <typeparam name="T">Tipo de dados da lista.</typeparam>
        /// <param name="conn">Objeto de conexão com a base de dados.</param>
        /// <param name="dataList">Lista de dados a ser enviado via Bulk Insert.</param>
        /// <param name="destinationTableName">Nome da tabela de destino.</param>
        public static void BulkInsertTransacoes<T>(this IDbConnection conn,
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
