using ProcessadorPassagensActors.CommandQuery.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class TagGrupoPracaValidator
    {
        private readonly ObterCountGrupoPorTagPracaQuery _countGrupoPorTagPracaQuery;
        private readonly ObterGrupoPorTagPracaQuery _grupoPorTagPracaQuery;

        public TagGrupoPracaValidator()
        {
            _countGrupoPorTagPracaQuery = new ObterCountGrupoPorTagPracaQuery();
            _grupoPorTagPracaQuery = new ObterGrupoPorTagPracaQuery();
        }

        public bool Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var resultCount = DataBaseConnection.HandleExecution(_countGrupoPorTagPracaQuery.Execute, passagemPendenteArtesp);

            if (resultCount)
                passagemPendenteArtesp.Tag.GrupoPadraoId = DataBaseConnection.HandleExecution(_grupoPorTagPracaQuery.Execute, passagemPendenteArtesp);

            if (passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.GrupoIsento
                && passagemPendenteArtesp.Tag.Grupo != Grupo.IsentoPelaArtesp
                && passagemPendenteArtesp.Tag.Grupo != Grupo.Isento)
            {
                return true;
            }
            return false;
        }
    }
}
