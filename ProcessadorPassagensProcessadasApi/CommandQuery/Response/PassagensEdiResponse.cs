using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public abstract class PassagensEdiResponse
    {
        public Guid ExecucaoId { get; set; }
        public bool SucessoStagingConectSys { get; set; }
        public TimeSpan TempoExecucaoStagingConectSys { get; set; }
        public bool SucessoStagingMensageria { get; set; }
        public TimeSpan TempoExecucaoStagingMensageria { get; set; }
        public bool SucessoConectSys { get; set; }
        public string ErroConectSys { get; set; }
        public TimeSpan TempoExecucaoConectSys { get; set; }
        public bool SucessoMensageria { get; set; }
        public string ErroMensageria { get; set; }
        public TimeSpan TempoExecucaoMensageria { get; set; }
    }
}
