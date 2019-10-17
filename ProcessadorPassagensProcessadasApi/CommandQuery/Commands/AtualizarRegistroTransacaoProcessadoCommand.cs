using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class AtualizarRegistroTransacaoProcessadoCommand : DbConnectionCommandBase<AtualizarRegistroTransacaoProcessadoArgs>
    {
        public AtualizarRegistroTransacaoProcessadoCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }

        public override void Execute(AtualizarRegistroTransacaoProcessadoArgs args)
        {
            args.RegistroTransacaoIds.Add(2004);
            const string query =
                @"UPDATE RegistroTransacao
                  SET Processado = 1 
                  WHERE RegistroTransacao.RegistroTransacaoId = @registroTransacaoId";


            DataSource.Connection.Execute(
                sql: query,
                transaction: DataSource.IsTransactional ? DataSource.Transaction : null,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.Text,
                param: args.RegistroTransacaoIds.Select(x => new {
                    registroTransacaoId = x
                }));

        }
    }
}
