using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class AtualizarFalhaPassagensReprovadasCommand : DbConnectionCommandBase<AtualizarFalhaPassagensReprovadasArgs>
    {
        public AtualizarFalhaPassagensReprovadasCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }
        
        public override void Execute(AtualizarFalhaPassagensReprovadasArgs args)
        {                        
            const string query = "SP_AtualizarFalhaPassagensReprovadas ";            

            var dto = DataSource.Connection.ExecuteScalar(
                sql: query,
                transaction: DataSource.IsTransactional ? DataSource.Transaction : null,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    ExecucaoId = args.ExecucaoId,
                    Motivo = args.Motivo                    
                });

        }
    }

}
