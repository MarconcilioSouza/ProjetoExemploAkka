using System;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class TransacaoPassagemDto
    {
        public long PassagemId { get; set; }
        public DateTime DataPagamento { get; set; }
        public long TransacaoId { get; set; }
        public bool ValePedagio { get; set; }
        public decimal ValorRepasse { get; set; }
    }
}
