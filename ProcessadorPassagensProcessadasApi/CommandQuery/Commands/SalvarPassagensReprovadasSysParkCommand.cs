//using AutoMapper;
//using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
//using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
//using ConectCar.Transacoes.Domain.Model;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
//using System.Collections.Generic;
//using System.Linq;
//using System;

//namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
//{
//    public class SalvarPassagensReprovadasSysParkCommand : DbConnectionCommandBase<PassagemReprovadaSysParkArgs>
//    {
//        public SalvarPassagensReprovadasSysParkCommand(DbConnectionDataSource dataSource) : base(dataSource)
//        {

//        }

//        public override void Execute(PassagemReprovadaSysParkArgs args)
//        {
//            var _transacoesRecusadas = args.TransacoesRecusadas != null ? args.TransacoesRecusadas.Where(x => x != null) : null;
//            var _detalheTransacao = args.DetalheTransacoes != null ? args.DetalheTransacoes.Where(x => x != null) : null;

//            var TransacoesRecusadas = Mapper.Map<IEnumerable<TransacaoEstacionamentoRecusada>, IEnumerable<TransacaoEstacionamentoRecusadaLote>>(_transacoesRecusadas);
//            var DetalheTransacao = Mapper.Map<IEnumerable<DetalheTransacaoEstacionamentoRecusada>, IEnumerable<DetalheTransacaoEstacionamentoRecusadaLote>>(_detalheTransacao);

//            if (TransacoesRecusadas != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(TransacoesRecusadas.ToList(), "TransacaoEstacionamentoRecusadaStaging");
//            }

//            if (DetalheTransacao != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(DetalheTransacao.ToList(), "DetalhePassagemEstacionamentoRecusadaStaging");
//            }
//        }
//    }
//}
