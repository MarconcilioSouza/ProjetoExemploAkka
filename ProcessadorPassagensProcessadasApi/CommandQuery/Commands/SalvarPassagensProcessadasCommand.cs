using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using Dapper;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class SalvarPassagensProcessadasCommand : DbConnectionCommandBase<PassagemProcessadaFilter, ProcedureStatusDto>
    {
        public SalvarPassagensProcessadasCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }

        public override ProcedureStatusDto Execute(PassagemProcessadaFilter filter)
        {
            
            var passagem = Mapper.Map<IEnumerable<PassagemProcessadaLoteStaging>>((filter.PassagensProcessadas ?? new List<PassagemProcessadaArtespDto>())).ToDataTable().AsTableValuedParameter("PassagemProcessadaLote");

            var query = "SP_SalvarPassagensProcessadas ";
            var dto = DataSource.Connection.QueryFirst<ProcedureStatusDto>(
                sql: query,
                transaction: DataSource.IsTransactional ? DataSource.Transaction : null,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    passagem = passagem,
                    ExecucaoId = filter.ExecucaoId
                });

            return dto;
        }
    }
}
