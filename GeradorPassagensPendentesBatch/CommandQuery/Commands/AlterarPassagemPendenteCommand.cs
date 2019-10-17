using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using GeradorPassagensPendentesBatch.CommandQuery.Commands.CommandsArgs;
using System.Data;

namespace GeradorPassagensPendentesBatch.CommandQuery.Commands
{
    public class AlterarPassagemPendenteCommand : DbConnectionCommandBase<AlterarPassagemPendenteCommandArg, bool>
    {
        public AlterarPassagemPendenteCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(AlterarPassagemPendenteCommandArg args)
        {

            var ret = DataSource.Connection.Execute("spAlterarTtlMensagemPendenteProcessamento", new {
                DataTtl = args.DataTtl,
                MensagemItemIdMin = args.MensagemItemIdMin,
                MensagemItemIdMax = args.MensagemItemIdMax

            }, DataSource.IsTransactional
               ? DataSource.Transaction : null,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600);

            return ret > 0;
        }
    }
}
