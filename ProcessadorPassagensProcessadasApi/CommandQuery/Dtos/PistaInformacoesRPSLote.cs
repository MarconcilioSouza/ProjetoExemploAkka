using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class PistaInformacoesRPSLote
    {
        public int? Id { get; set; }
        public string SerieRPS { get; set; }
        public long NumeroRPS { get; set; }
        public DateTime DataCriacao { get; set; }
        public int PistaId { get; set; }
        public int ConveniadoInformacoesRPSId { get; set; }
        public Guid SurrogateKey { get; set; }
    }
}
