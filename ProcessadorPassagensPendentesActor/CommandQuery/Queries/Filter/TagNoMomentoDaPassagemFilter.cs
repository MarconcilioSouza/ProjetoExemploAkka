using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
   public class TagNoMomentoDaPassagemFilter
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public int TempoAtualizacaoPista { get; set; }
        public bool ExistePassagemAnterior { get; set; }
    }
}
