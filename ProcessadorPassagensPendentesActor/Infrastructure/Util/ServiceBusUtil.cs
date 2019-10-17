using System;
using System.Configuration;
using static ProcessadorPassagensActors.Infrastructure.EnumInfra;

namespace ProcessadorPassagensActors.Infrastructure.Util
{

    public static class ServiceBusUtil
    {
        public static int FactoriesCount = 1;
        private static DateTime _dataBaseHealthyCheckDateTime = DateTime.Now;

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


        /// <summary>
        /// Verifica se é necessário realizar a checagem da saúde do banco de dados.
        /// </summary>
        /// <returns>True caso seja necessário realizar a checagem da base de dados.</returns>
        public static bool DataBaseHealthyCheck()
        {
            if(_dataBaseHealthyCheckDateTime <= DateTime.Now)
            {
                var habilitarDataBaseHealthyCheck = ConfigurationManager.AppSettings["HabilitarDataBaseHealthyCheck"];
                if (habilitarDataBaseHealthyCheck == null)
                    return true;
                else
                {
                    return habilitarDataBaseHealthyCheck == "1";
                }
            }

            return false;            
        }

        /// <summary>
        /// Atualiza a data de checagem da saúde da base de dados para os próximos 10 minutos.
        /// </summary>
        public static void UpdateDataBaseHealthyCheckDateTime()
        {
            _dataBaseHealthyCheckDateTime = DateTime.Now.AddSeconds(TimeHelper.CommandTimeOut);
        }
    }

}
