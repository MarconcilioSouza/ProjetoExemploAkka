using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalhePassagemEstacionamentoLoteStaging
    {
        public int Id { get; set; }
        public DateTime DataHoraPassagem { get; set; }
        public int PistaId { get; set; }
        public int TransacaoEstacionamentoId { get; set; }
        public long SurrogateKey { get; set; }
    }
}
