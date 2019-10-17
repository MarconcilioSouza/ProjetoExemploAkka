using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensReprovadasArtespResponse: PassagensArtespResponse
    {        
        public int QtdPassagensProcessadasStaging { get; set; }
        public int QtdVeiculosStaging { get; set; }
        public int QtdPassagensStaging { get; set; }
        public int QtdTransacoesRecusadasStaging { get; set; }
        public int QtdTransacoesRecusadasParceiroStaging { get; set; }
    }
}
