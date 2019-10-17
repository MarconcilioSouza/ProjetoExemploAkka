using ProcessadorPassagensActors.CommandQuery.Enums;
using System.Configuration;

namespace ProcessadorPassagensActors.CommandQuery.Util
{
    public static class ServiceBusUtil
    {
        public const int BatchSize = 500;
        public const int FactoriesCount = 40;

        /// <summary>
        ///  Obtem o nome do topic a ser enviada para o barramento para passagens aprovadas.
        /// </summary>
        /// <param name="protocolo"></param>
        /// <returns></returns>
        public static string ObterNomeTopicAprovada(ProtocolosEnum protocolo)
        {
            var nomesTopic = ConfigurationManager.AppSettings["PassagensAprovadasPadrao"];
            switch (protocolo)
            {
                case ProtocolosEnum.PassagensAprovadasArtesp:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensAprovadasArtesp"];
                    break;
                case ProtocolosEnum.PassagensAprovadasEdi:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensAprovadasEdi"];
                    break;
                case ProtocolosEnum.PassagensAprovadasPark:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensAprovadasPark"];
                    break;
            }
            return nomesTopic;
        }

        /// <summary>
        /// Obtem o nome do topic a ser enviada para o barramento para passagens reprovadas.
        /// </summary>
        public static string ObterNomeTopicReprovada(ProtocolosEnum protocolo)
        {
            var nomesTopic = ConfigurationManager.AppSettings["PassagensReprovadasPadrao"];
            switch (protocolo)
            {
                case ProtocolosEnum.PassagensReprovadasArtesp:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensReprovadasArtesp"];
                    break;
                case ProtocolosEnum.PassagensReprovadasEDI:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensReprovadasEdi"];
                    break;
                case ProtocolosEnum.PassagensReprovadasPark:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensReprovadasPark"];
                    break;
            }
            return nomesTopic;
        }

        /// <summary>
        /// Obtem o nome do topic a ser enviada para o barramento para passagens invalidas.
        /// </summary>
        public static string ObterNomeTopicInvalida(ProtocolosEnum protocolo)
        {
            var nomesTopic = ConfigurationManager.AppSettings["PassagensInvalidasPadrao"];
            switch (protocolo)
            {
                case ProtocolosEnum.PassagensReprovadasArtesp:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensInvalidasArtesp"];
                    break;
                case ProtocolosEnum.PassagensReprovadasEDI:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensInvalidasEdi"];
                    break;
                case ProtocolosEnum.PassagensReprovadasPark:
                    nomesTopic = ConfigurationManager.AppSettings["PassagensInvalidasPark"];
                    break;
            }
            return nomesTopic;
        }

    }
}
