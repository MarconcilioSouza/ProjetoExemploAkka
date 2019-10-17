using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using System.Collections.Generic;
using Transacoes.Centralizadas.Autorizacao.Backend.ConsolidadorTransacoesApi.CommandQuery.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.Extensions;
using System.Data;

namespace Transacoes.Centralizadas.Autorizacao.Backend.ConsolidadorTransacoesApi.CommandQuery.Commands
{
    public class SalvarPassagensInvalidasCommand : DbConnectionCommandBase<List<PassagemInvalidaModel>>
    {
        public SalvarPassagensInvalidasCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
            
        }

        public override void Execute(List<PassagemInvalidaModel> ListArgs)
        {
            DataSource.Connection.BulkInsert(ListArgs, "ItemFalhaProcessamentoHistorico");
        }
    }
}
