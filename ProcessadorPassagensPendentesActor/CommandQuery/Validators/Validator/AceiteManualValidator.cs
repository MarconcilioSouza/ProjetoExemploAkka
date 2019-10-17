using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class AceiteManualValidator
    {
        private PassagemPendenteArtesp PassagemPendenteArtesp { get; }
        public AceiteManualValidator(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
        }

        public void Validate(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack)
        {
            var queryCountAceitManual =
                new ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery();
            var aceiteManualId = DataBaseConnection.HandleExecution(queryCountAceitManual.Execute,
                new AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter(PassagemPendenteArtesp.ConveniadoPassagemId, PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp));
            if (aceiteManualId > 0)
            {
                PassagemPendenteArtesp.PossuiAceiteManualReenvioPassagem = true;
            }
        }

    }
}
