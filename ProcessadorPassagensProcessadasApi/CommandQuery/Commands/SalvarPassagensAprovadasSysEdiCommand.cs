using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System.Collections.Generic;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class SalvarPassagensAprovadasSysEdiCommand : DbConnectionCommandBase<PassagemAprovadaEdiSysArgs>
    {
        public SalvarPassagensAprovadasSysEdiCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(PassagemAprovadaEdiSysArgs args)
        {
            #region Group Domain
            var transacoesPassagens = args.TransacoesPassagens?.Where(x => x != null);
            var detalheTrfRecusado = args.DetalheTRFRecusado?.Where(x => x != null);
            var transacaoProvisoria = args.TransacaoProvisoria?.Where(x => x != null);
            var detalheTrfAprovadoManualmente = args.DetalheTRFAprovadoManualmente?.Where(x => x != null);
            var extrato = args.Extrato?.Where(x => x != null);
            var evento = args.Evento?.Where(x => x != null);
            var configuracaoAdesao = args.ConfiguracaoAdesao?.Where(x => x != null);
            var divergenciaCategoriaConfirmada = args.DivergenciaCategoriaConfirmada?.Where(x => x != null);
            var veiculo = args.Veiculo?.Where(x => x != null);
            var detalheViagem = args.DetalheViagem?.Where(x => x != null);

            if (transacoesPassagens == null) return;
            #endregion

            #region Mapper to Staging
            var transacoesPassagemStaging = Mapper.Map<List<TransacaoPassagemEDIDto>, List<TransacaoPassagemLoteStaging>>(transacoesPassagens.ToList());
            var transacaosProvisoriaStaging = Mapper.Map<IEnumerable<TransacaoProvisoriaEDIDto>, IEnumerable<TransacaoPassagemLoteStaging>>(transacaoProvisoria);
            if (transacaosProvisoriaStaging != null)
                transacoesPassagemStaging.AddRange(transacaosProvisoriaStaging);

            var detalheTrfRecusadoLoteStaging = Mapper.Map<IEnumerable<DetalheTRFRecusadoDto>, IEnumerable<DetalheTRFRecusadoLoteStaging>>(detalheTrfRecusado);
            var detalheTrfAprovadoManualmenteLoteStaging = Mapper.Map<IEnumerable<DetalheTRFAprovadoManualmenteDto>, IEnumerable<DetalheTRFAprovadaManualmenteLoteStaging>>(detalheTrfAprovadoManualmente);
            var extratoLoteStaging = Mapper.Map<IEnumerable<ExtratoDto>, IEnumerable<ExtratoLoteStaging>>(extrato);
            var eventoLoteStaging = Mapper.Map<IEnumerable<EventoDto>, IEnumerable<EventoLoteStaging>>(evento);
            var configuracaoAdesaoLoteStaging = Mapper.Map<IEnumerable<ConfiguracaoAdesaoDto>, IEnumerable<ConfiguracaoAdesaoLoteStaging>>(configuracaoAdesao);
            var diveregenciaCategoriaConfirmadaLoteStaging = Mapper.Map<IEnumerable<DivergenciaCategoriaConfirmadaDto>, IEnumerable<DivergenciaCategoriaConfirmadaLoteStaging>>(divergenciaCategoriaConfirmada);
            var veiculoLoteStaging = Mapper.Map<IEnumerable<VeiculoDto>, IEnumerable<VeiculoLoteStaging>>(veiculo);
            var detalheViagemLoteStaging = Mapper.Map<IEnumerable<DetalheViagemDto>, IEnumerable<DetalheViagemLoteStaging>>(detalheViagem);
            #endregion

            #region Bulk Insert
            if (transacoesPassagemStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(transacoesPassagemStaging.ToList(), "TransacaoPassagemEdiStaging");
            }

            if (detalheTrfRecusadoLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(detalheTrfRecusadoLoteStaging.ToList(), "DetalheTRFRecusadoLoteStaging");
            }

            if (detalheTrfAprovadoManualmenteLoteStaging != null && args.DetalheTRFAprovadoManualmente.Any())
            {
                DataSource.Connection.BulkInsertTransacoes(detalheTrfAprovadoManualmenteLoteStaging.ToList(), "DetalheTRFAprovadaManualmenteLoteStaging");
            }

            if (extratoLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(extratoLoteStaging.ToList(), "ExtratoLoteStaging"); // não entendi
            }

            if (eventoLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(eventoLoteStaging.ToList(), "EventoLoteStaging");
            }

            if (configuracaoAdesaoLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(configuracaoAdesaoLoteStaging.ToList(), "ConfiguracaoAdesaoLoteStaging");
            }

            if (diveregenciaCategoriaConfirmadaLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(diveregenciaCategoriaConfirmadaLoteStaging.ToList(), "DivergenciaCategoriaConfirmadaLoteStaging");
            }

            if (veiculoLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(veiculoLoteStaging.ToList(), "VeiculoLoteStaging");
            }

            if (detalheViagemLoteStaging != null)
            {
                DataSource.Connection.BulkInsertTransacoes(detalheViagemLoteStaging.ToList(), "DetalheViagemLoteStaging");
            }
            #endregion
        }
    }
}