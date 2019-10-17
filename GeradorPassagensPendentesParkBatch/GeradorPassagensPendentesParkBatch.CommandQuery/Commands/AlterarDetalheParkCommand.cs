using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Commands
{
    public class AlterarDetalheParkCommand : DbConnectionCommandBase<AlterarDetalheParkCommandArgs, bool>
    {
        public AlterarDetalheParkCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(AlterarDetalheParkCommandArgs args)
        {
            var ret = DataSource.Connection.Execute("spAlterarTtlRegistroTransacao", new
            {
                DataTtl = args.DataTtl,
                RegistroTransacaoIdMin = args.RegistroTransacaoIdMin,
                RegistroTransacaoIdMax = args.RegistroTransacaoIdMax
            }, DataSource.IsTransactional
               ? DataSource.Transaction : null,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600);

            return ret > 0;
        }
    }
}