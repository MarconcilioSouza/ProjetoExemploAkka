using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoRecusadaLote
    {
        public Guid ExecucaoId { get; set; }
        public int? MotivoRecusadoId { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public int? PassagemId { get; set; }
        public long? SurrogateKey { get; set; }
    }
}
