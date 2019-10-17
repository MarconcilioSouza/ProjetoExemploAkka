namespace GeradorPassagensPendentesParkBatch.CommandQuery.Util
{
    public static class ServiceBusUtil
    {
        public static int FactoriesCount = 5;

        /// <summary>
        /// Configura o nome do topic a ser criado no barramento
        /// </summary>
        /// <returns></returns>
        public static string ObterNomeTopicPassagem()
        {
            return System.Configuration.ConfigurationManager.AppSettings["PassagensPendentesPark"];
        }
    }
}