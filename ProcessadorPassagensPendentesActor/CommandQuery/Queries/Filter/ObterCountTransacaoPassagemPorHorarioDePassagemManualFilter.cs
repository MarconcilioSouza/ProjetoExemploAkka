using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
   public class ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        
        public int tempoLimite { get; set; }
    }
}
