using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class GrupoIsentoTagValidator : Loggable
    {        
        private readonly ObterGrupoPorTagPracaQuery _grupoPorTagPracaQuery;

        public GrupoIsentoTagValidator()
        {
            _grupoPorTagPracaQuery = new ObterGrupoPorTagPracaQuery();            
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {            
            var tagGrupoPraca = DataBaseConnection.HandleExecution(_grupoPorTagPracaQuery.Execute,passagemPendenteArtesp);

            if (tagGrupoPraca > 0)
                passagemPendenteArtesp.Tag.GrupoPadraoId = tagGrupoPraca;
            

            if ((passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.GrupoIsento) 
                && passagemPendenteArtesp.Tag.Grupo != Grupo.IsentoPelaArtesp 
                && passagemPendenteArtesp.Tag.Grupo != Grupo.Isento)
                return MotivoNaoCompensado.TagNaoPertenceGrupoIsento;

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }
    }
}
