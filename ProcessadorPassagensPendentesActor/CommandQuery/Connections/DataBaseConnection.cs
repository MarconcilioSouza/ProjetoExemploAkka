using System;
using System.Configuration;
using System.Data.SqlClient;
using Common.Logging;
using ConectCar.Framework.Infrastructure.Cqrs.Ado;
using Dapper;
using Polly;
using Polly.Retry;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Connections
{
    public enum DataBaseSourceType
    {
        ConectSys,
        ConectSysReadOnly,
        Mensageria,
        ConectPark
    }

    public static class DataBaseConnection
    {
        private static DateTime _healthyExpiration;
        private static string _connectionString;
        private static string _connectionStringReadOnly;
        private static string _connectionStringMensageria;
        private static string _connectionStringPark;
        private static bool _useReadOnly;
        private static ILog _log = LogManager.GetLogger(typeof(DataBaseConnection));
        private static RetryPolicy _retryPolicy = Policy
                .Handle<Exception>()
                .Retry(1, (exception, retryCount, context) => SetHealthyExpiration(false, TimeHelper.LagSeconds));
        private static Object _locker = new Object();


        static DataBaseConnection()
        {
            //ConectSysReadOnlyConnStr
            var connStr = ConfigurationManager.ConnectionStrings["ConectSysConnStr"];
            _connectionString = connStr.ConnectionString;

            var connStrReadOnly = ConfigurationManager.ConnectionStrings["ConectSysReadOnlyConnStr"];
            _connectionStringReadOnly = connStrReadOnly.ConnectionString;

            var connStrMensageria = ConfigurationManager.ConnectionStrings["MensageriaConnStr"];
            _connectionStringMensageria = connStrMensageria.ConnectionString;

            _healthyExpiration = DateTime.Now;
            _useReadOnly = false;
        }

        public static SqlConnection GetConnection(DataBaseSourceType dataSourceType)
        {
            switch (dataSourceType)
            {
                case DataBaseSourceType.ConectSys:
                    return new SqlConnection(_connectionString);                    
                case DataBaseSourceType.ConectSysReadOnly:
                    return new SqlConnection(_connectionStringReadOnly);
                case DataBaseSourceType.Mensageria:
                    return new SqlConnection(_connectionStringMensageria);
                case DataBaseSourceType.ConectPark:
                    return new SqlConnection(_connectionStringPark);               
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(dataSourceType)}", "Opção inválida.");
            }
        }

        public static SqlConnection GetConnection(bool useReadOnly, int lagSeconds)
        {
            // se não é para usar readonly, usa a base principal
            if (!useReadOnly)
                return new SqlConnection(_connectionString);

            // caso a data de expiração de avaliação ainda não venceu, retorna a conexão corrente
            if (_healthyExpiration > DateTime.Now)
                return new SqlConnection(GetConnection());

            // verifica a integridade da base
            var isHealthy = IsHealthy(lagSeconds);
            SetHealthyExpiration(isHealthy, lagSeconds);            

            return new SqlConnection(GetConnection());
        }

        /// <summary>
        /// Atualiza a variável de utilização do readonly e tempo de expiração da conexão.
        /// </summary>
        /// <param name="useReadOnly">True se é para utilizar a base de leitura.</param>
        /// <param name="lagSeconds">Tempo em segundos para considerar a conexão atual como saudável.</param>
        public static void SetHealthyExpiration(bool useReadOnly, int lagSeconds)
        {
            lock (_locker)
            {
                _useReadOnly = useReadOnly;
                _healthyExpiration = DateTime.Now.AddSeconds(lagSeconds);
            }
            
        }

        public static TReturn HandleExecution<TReturn>(Func<TReturn> handler)
        {
            return _retryPolicy.Execute(() => handler());
        }

        public static TReturn HandleExecution<TParam, TReturn>(Func<TParam, TReturn> handler, TParam param)
        {
            return _retryPolicy.Execute(() => handler(param));
        }

        /// <summary>
        /// Obtem a string de conexão com base no status de uso da leitura.
        /// </summary>
        /// <returns>String de conexão.</returns>
        private static string GetConnection()
        {
            return _useReadOnly ? _connectionStringReadOnly : _connectionString;
        }

        /// <summary>
        /// Verifica se a base de dados de réplica está saudável.
        /// </summary>
        /// <param name="lagSeconds">Tempo de divergência.</param>
        /// <returns>True caso esteja saudável.</returns>
        private static bool IsHealthy(int lagSeconds)
        {
            _log.Debug($"Verificando a integridade da base de dados de leitura.");

            using (var conn = new SqlConnection(_connectionString))
            {
                var dataSourceServer = conn.Database;
                var replicaHealthy = conn.QueryFirstOrDefault<ReplicaHealthy>(@"
                    WITH DR_CTE ( replica_server_name
                        , database_name
                        , last_commit_time)
                    AS
                    (
                                select ar.replica_server_name, database_name, rs.last_commit_time
                                from master.sys.dm_hadr_database_replica_states  rs
                                inner join master.sys.availability_replicas ar on rs.replica_id = ar.replica_id
                                inner join sys.dm_hadr_database_replica_cluster_states dcs on dcs.group_database_id = rs.group_database_id and rs.replica_id = dcs.replica_id
                                
                    )

                    SELECT TOP 1 
                        DR_CTE.replica_server_name                                      AS 'ReplicaServerName'
                        , dcs.database_name                                             AS 'DatabaseName'
                        , rs.last_commit_time                                           AS 'LastCommitTime'
                        , DR_CTE.last_commit_time                                       AS 'ReplicaCommitTime'
                        , datediff(ss,  DR_CTE.last_commit_time, rs.last_commit_time)   AS 'LagSeconds'
                    FROM master.sys.dm_hadr_database_replica_states  rs
                    INNER JOIN master.sys.availability_replicas ar ON rs.replica_id = ar.replica_id
                    INNER JOIN sys.dm_hadr_database_replica_cluster_states dcs ON dcs.group_database_id = rs.group_database_id 
                        AND rs.replica_id = dcs.replica_id
                    INNER JOIN DR_CTE ON DR_CTE.database_name = dcs.database_name
                    WHERE dcs.database_name = @serverName
                    ORDER BY LagSeconds DESC", new { serverName = dataSourceServer });

                if (replicaHealthy == null)
                {
                    _log.Debug("Não foi possível determinar a saúde da base de leitura. Será utilizada a base principal.");
                    return false;
                }
                    
                var isHealthy = replicaHealthy.LagSeconds <= lagSeconds;
                if(isHealthy)
                    _log.Debug($"Base de leitura será utilizada pelos próximos {lagSeconds} segundos.");
                else
                    _log.Debug($"Base de leitura desatualizada em {replicaHealthy.LagSeconds} segundos. Será utilizada a base principal.");

                return isHealthy;
            }
                            
        }


    }
}
