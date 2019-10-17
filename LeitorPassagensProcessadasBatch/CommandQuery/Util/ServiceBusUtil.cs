using LeitorPassagensProcessadasBatch.CommandQuery.Enum;
using static System.Configuration.ConfigurationManager;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Util
{
    public static class ServiceBusUtil
    {
        private const string PassagensAprovadasArtesp = "PassagensAprovadasArtesp";
        private const string PassagensReprovadasArtesp = "PassagensReprovadasArtesp";
        private const string PassagensInvalidasArtesp = "PassagensInvalidasArtesp";

        private const string PassagensAprovadasEdi = "PassagensAprovadasEDI";
        private const string PassagensReprovadasEdi = "PassagensReprovadasEDI";

        private const string PassagensAprovadasPark = "PassagensAprovadasPark";
        private const string PassagensReprovadasPark = "PassagensReprovadasPark";

        private const string PassagensProcessadasPadrao = "PassagensProcessadasPadrao";

        public const int BatchSize = 500;
        public const int FactoriesCount = 40;
        public static string ObterNome(TypeTransacao tipoTransacao)
        {
            switch (tipoTransacao)
            {
                case TypeTransacao.AprovadaArtesp:
                    return AppSettings[PassagensAprovadasArtesp];
                case TypeTransacao.ReprovadaArtesp:
                    return AppSettings[PassagensReprovadasArtesp];
                case TypeTransacao.InvalidaArtesp:
                    return AppSettings[PassagensInvalidasArtesp];

                case TypeTransacao.AprovadaEdi:
                    return AppSettings[PassagensAprovadasEdi];
                case TypeTransacao.ReprovadaEdi:
                    return AppSettings[PassagensReprovadasEdi];

                case TypeTransacao.AprovadaPark:
                    return AppSettings[PassagensAprovadasPark];
                case TypeTransacao.ReprovadaPark:
                    return AppSettings[PassagensReprovadasPark];

                default:
                    return AppSettings[PassagensProcessadasPadrao];
            }
        }
    }
}
