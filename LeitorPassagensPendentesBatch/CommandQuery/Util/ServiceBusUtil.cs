using LeitorPassagensPendentesBatch.CommandQuery.Enum;
using System.Configuration;

namespace LeitorPassagensPendentesBatch.CommandQuery.Util
{
    public static class ServiceBusUtil
    {
        public const int BatchSize = 500;
        public const int FactoriesCount = 40;

        /// <summary>
        /// Configura o nome da fila a ser criada no barramento
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static string ObterNome(ProtocoloEnum topic)
        {
            string nomesTopic = ConfigurationManager.AppSettings["PassagensPendentesPadrao"].ToString();
            switch (topic)
            {
                case ProtocoloEnum.PassagensPendentesPadrao:
                    break;
                case ProtocoloEnum.PassagensPendentesArtesp:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensPendentesArtesp"].ToString();
                    break;
                case ProtocoloEnum.PassagensPendentesEdi:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensPendentesEdi"].ToString();
                    break;
                case ProtocoloEnum.PassagensPendentesPark:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensPendentesPark"].ToString();
                    break;
            }
            return nomesTopic;
        }
    }
}
