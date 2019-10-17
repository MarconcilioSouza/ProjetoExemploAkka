using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using Dapper;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class SalvarPassagensAprovadasSysCommand : DbConnectionCommandBase<PassagemAprovadaSysFilter, ProcedureStatusDto>
    {
        public SalvarPassagensAprovadasSysCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }
        
        public override ProcedureStatusDto Execute(PassagemAprovadaSysFilter filter)
        {                        
            const string query = "SP_SalvarPassagensAprovadas ";            

            var args = new SalvarPassagensAprovadasArgs
            {
                aceiteManualReenvioPassagem = Mapper.Map<IEnumerable<AceiteManualReenvioPassagemLote>>((filter.AceitesManuaisReenvioPassagem ?? new List<AceiteManualReenvioPassagemDto>())).ToDataTable().AsTableValuedParameter("AceiteManualReenvioPassagemLote"),
                configuracaoAdesao = Mapper.Map<IEnumerable<ConfiguracaoAdesaoLoteStaging>>((filter.ConfiguracoesAdesao ?? new List<ConfiguracaoAdesaoDto>())).ToDataTable().AsTableValuedParameter("ConfiguracaoAdesaoLote"),
                divergenciasCategoriasConfirmadas = Mapper.Map<IEnumerable<DivergenciaCategoriaConfirmadaLoteStaging>>((filter.DivergenciasCategoriaConfirmada ?? new List<DivergenciaCategoriaConfirmadaDto>())).ToDataTable().AsTableValuedParameter("DivergenciaCategoriaConfirmadaLote"),
                estonosPassagens = Mapper.Map<IEnumerable<EstornoPassagemLote>>((filter.EstornosPassagem ?? new List<EstornoPassagemDto>())).ToDataTable().AsTableValuedParameter("EstornoPassagemLote"),
                extratosEstornos = Mapper.Map<IEnumerable<ExtratoLoteStaging>>((filter.ExtratosEstornos ?? new List<ExtratoDto>())).ToDataTable().AsTableValuedParameter("ExtratoLote"),
                eventos = Mapper.Map<IEnumerable<EventoLoteStaging>>((filter.Eventos ?? new List<EventoDto>())).ToDataTable().AsTableValuedParameter("EventoLote"),
                extratos = Mapper.Map<IEnumerable<ExtratoLoteStaging>>((filter.Extratos ?? new List<ExtratoDto>())).ToDataTable().AsTableValuedParameter("ExtratoLote"),
                passagens = Mapper.Map<IEnumerable<PassagemLoteStaging>>((filter.Passagens ?? new List<PassagemDto>())).ToDataTable().AsTableValuedParameter("PassagemLote"),
                solicitacoesImagens = Mapper.Map<IEnumerable<SolicitacaoImagemLote>>((filter.SolicitacoesImagem ?? new List<SolicitacaoImagemDto>())).ToDataTable().AsTableValuedParameter("SolicitacaoImagemLote"),
                transacaoPassagem = Mapper.Map<IEnumerable<TransacaoPassagemLoteStaging>>((filter.TransacoesPassagens ?? new List<TransacaoPassagemDto>())).ToDataTable().AsTableValuedParameter("TransacaoPassagemLote"),
                veiculos = Mapper.Map<IEnumerable<VeiculoLoteStaging>>((filter.Veiculos ?? new List<VeiculoDto>())).ToDataTable().AsTableValuedParameter("VeiculoLote"),
                viagensValePedagio = Mapper.Map<IEnumerable<DetalheViagemLoteStaging>>((filter.DetalhesViagem ?? new List<DetalheViagemDto>())).ToDataTable().AsTableValuedParameter("DetalheViagemLote"),
            };


            var dto = DataSource.Connection.QueryFirst<ProcedureStatusDto>(
                sql: query,
                transaction: DataSource.IsTransactional ? DataSource.Transaction : null,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    ExecucaoId = filter.ExecucaoId,
                    passagens = args.passagens,
                    transacaoPassagem = args.transacaoPassagem,
                    extratos = args.extratos,
                    extratosEstornos = args.extratosEstornos,
                    estonosPassagens = args.estonosPassagens,
                    veiculos = args.veiculos,
                    eventos = args.eventos,
                    viagensValePedagio = args.viagensValePedagio,
                    solicitacoesImagens = args.solicitacoesImagens,
                    aceiteManualReenvioPassagem = args.aceiteManualReenvioPassagem,
                    configuracaoAdesao = args.configuracaoAdesao,
                    divergenciasCategoriasConfirmadas = args.divergenciasCategoriasConfirmadas
                });

            return dto;
        }
    }

}
