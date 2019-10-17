using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterPassagemCompletaFilter
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; }
        public long PassagemId { get; }

        public ObterPassagemCompletaFilter(PassagemPendenteArtesp passagemPendenteArtesp, long passagemId)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
            PassagemId = passagemId;
        }      
    }
}
