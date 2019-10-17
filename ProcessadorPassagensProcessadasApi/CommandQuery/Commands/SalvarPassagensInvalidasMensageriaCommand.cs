using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using Dapper;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public sealed class SalvarPassagensInvalidasMensageriaCommand : DbConnectionCommandBase<List<PassagemInvalidaArtespDto>>
    {
        public SalvarPassagensInvalidasMensageriaCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }

        public override void Execute(List<PassagemInvalidaArtespDto> PassagensInvalidas)
        {
            var dataTable = (PassagensInvalidas ?? new List<PassagemInvalidaArtespDto>()).ToDataTable().AsTableValuedParameter("PassagemInvalidaLote");
            const string query = "SP_SalvarPassagemInvalidas  ";
            DataSource.Connection.ExecuteScalar(
                sql: query,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.StoredProcedure,
                param: new { passagensInvalidas = dataTable });
        }
    }
}
