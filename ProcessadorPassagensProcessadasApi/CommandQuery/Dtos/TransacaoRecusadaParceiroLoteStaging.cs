using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoRecusadaParceiroLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public DateTime? DataEnvioAoParceiro { get; set; }
        public DateTime? DataPassagemNaPraca { get; set; }
        public int? ParceiroId { get; set; }
        public int? ViagemAgendadaId { get; set; }
        public decimal? Valor { get; set; }
        public long? SurrogateKey { get; set; }
        public int? StagingId { get; set; }
        public int? DetalheTrnId { get; set; }
        public int? CodigoRetornoTRFId { get; set; }
    }
}