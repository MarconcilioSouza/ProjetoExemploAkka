namespace GeradorPassagensPendentesBatch.CommandQuery.Util
{
    public static class ServiceBusUtil
    {
        public static int FactoriesCount = 5;
        public static int BatchFlushInterval = 100;
        public static int LockDuration = 2;
        /// <summary>
        /// Configura o nome da fila a ser criada no barramento
        /// </summary>
        /// <param name="codigoProtocoloArtesp">Id do conveniado/concessionaria</param>
        /// <returns></returns>
        public static string ObterNomeQueuePassagem()
        {
            return System.Configuration.ConfigurationManager.AppSettings["PassagensPendentesArtesp"];
        }
    }
}
