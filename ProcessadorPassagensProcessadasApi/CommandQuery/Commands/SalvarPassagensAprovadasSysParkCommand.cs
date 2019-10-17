//using AutoMapper;
//using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
//using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
//using ConectCar.Transacoes.Domain.Model;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
//using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
//using System.Collections.Generic;
//using System.Linq;

//namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
//{
//    public class SalvarPassagensAprovadasSysParkCommand : DbConnectionCommandBase<PassagemAprovadaParkSysArgs>
//    {
//        public SalvarPassagensAprovadasSysParkCommand(DbConnectionDataSource dataSource) : base(dataSource)
//        {

//        }

//        public override void Execute(PassagemAprovadaParkSysArgs args)
//        {
//            var _transacoes = args.Transacoes != null ? args.Transacoes.Where(x => x != null) : null;
//            var _detalhes = args.DetalhePassagens != null ? args.DetalhePassagens.Where(x => x != null) : null;
//            var _pistas = args.PistaInformacoes != null ? args.PistaInformacoes.Where(x => x != null) : null;
//            var _conveniados = args.ConveniadoInformacoes != null ? args.ConveniadoInformacoes.Where(x => x != null) : null;

//            var Transacao = Mapper.Map<IEnumerable<TransacaoEstacionamento>, IEnumerable<TransacaoEstacionamentoLote>>(_transacoes);
//            var Detalhe = Mapper.Map<IEnumerable<DetalhePassagemEstacionamento>, IEnumerable<DetalhePassagemEstacionamentoLote>>(_detalhes);
//            var Pista = Mapper.Map<IEnumerable<PistaInformacoesRps>, IEnumerable<PistaInformacoesRPSLote>>(_pistas);
//            var Conveniado = Mapper.Map<IEnumerable<ConveniadoInformacoesRps>, IEnumerable<ConveniadoInformacoesRpsLote>>(_conveniados);

//            if (Transacao != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(Transacao.ToList(), "TransacaoPassagemEstacionamentoStaging");
//            }

//            if (Detalhe != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(Detalhe.ToList(), "DetalhePassagemEstacionamentoStaging");
//            }

//            if (Pista != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(Pista.ToList(), "PistaInformacoesRpsStaging");
//            }

//            if (Conveniado != null)
//            {
//                DataSource.Connection.BulkInsertTransacoes(Conveniado.ToList(), "ConveniadoInformacoesRpsStaging");
//            }

//        }
//    }
//}
