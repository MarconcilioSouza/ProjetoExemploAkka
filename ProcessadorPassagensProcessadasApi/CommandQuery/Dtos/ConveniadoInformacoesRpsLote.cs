using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class ConveniadoInformacoesRpsLote
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public int? StagingId { get; set; }
        public int ConveniadoId { get; set; }
        public string SerieRPS { get; set; }
        public long NumeroRPS { get; set; }
        public int TipoRps { get; set; }
        public Guid SurrogateKey { get; set; }

    }
}
