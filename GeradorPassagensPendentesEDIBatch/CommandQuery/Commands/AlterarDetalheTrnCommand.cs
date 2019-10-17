using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Commands
{
    public class AlterarDetalheTrnCommand : DbConnectionCommandBase<AlterarDetalheTrnCommandArgs, bool>
    {
        public AlterarDetalheTrnCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(AlterarDetalheTrnCommandArgs args)
        {
            var ret = DataSource.Connection.Execute("spAlterarTtlDetalheTrn", new
            {
                DataTtl = args.DataTtl,
                DetalheTrnIdMin = args.DetalheTrnIdMin,
                DetalheTrnIdMax = args.DetalheTrnIdMax
            }, DataSource.IsTransactional
               ? DataSource.Transaction : null,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600);

            return ret > 0;
        }
    }
}