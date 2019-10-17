using ConectCar.Transacoes.Domain.Dto;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalheTransacaoEstacionamentoRecusadaLoteStaging
    {
        public int? PistaId { get; set; }
        public int? PracaId { get; set; }
        public DateTime DataHoraPassagem { get; set; }
        public long SurrogateKey { get; set; }
    }
}
