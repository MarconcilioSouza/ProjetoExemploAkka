using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class PistaInformacoesRPSLoteStaging 
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public int StagingId { get; set; }
        public string SerieRPS { get; set; }
        public long NumeroRPS { get; set; }
        public DateTime DataCriacao { get; set; }
        public int PistaId { get; set; }
        public int ConveniadoInformacoesRPSId { get; set; }
        //public long SurrogateKey { get; set; }
    }
}
