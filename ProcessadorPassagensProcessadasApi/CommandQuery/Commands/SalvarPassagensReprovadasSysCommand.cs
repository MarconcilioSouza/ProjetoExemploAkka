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

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public sealed class SalvarPassagensReprovadasSysCommand : DbConnectionCommandBase<PassagemReprovadaSysFilter, ProcedureStatusDto>
    {
        public SalvarPassagensReprovadasSysCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {

        }

        public override ProcedureStatusDto Execute(PassagemReprovadaSysFilter filter)
        {
            Log.Debug("Iniciando executação da procedure SP_SalvarPassagensReprovadas");

            var args = new SalvarPassagensReprovadasArgs
            {
                passagens = Mapper.Map<IEnumerable<PassagemLoteStaging>>((filter.Passagens ?? new List<PassagemDto>())).ToDataTable().AsTableValuedParameter("PassagemLote"),
                transacoesRecusadas = Mapper.Map<IEnumerable<TransacaoRecusadaLoteStaging>>((filter.TransacoesRecusada ?? new List<TransacaoRecusadaDto>())).ToDataTable().AsTableValuedParameter("TransacaoRecusadaLote"),
                transacoesRecusadasParceiros = Mapper.Map<IEnumerable<TransacaoRecusadaParceiroLoteStaging>>((filter.TransacoesRecusadaParceiro ?? new List<TransacaoRecusadaParceiroDto>())).ToDataTable().AsTableValuedParameter("TransacaoRecusadaParceiroLote"),
                veiculos = Mapper.Map<IEnumerable<VeiculoLoteStaging>>((filter.Veiculos ?? new List<VeiculoDto>())).ToDataTable().AsTableValuedParameter("VeiculoLote"),
            };


            const string query = "SP_SalvarPassagensReprovadas";
            var dto = DataSource.Connection.QueryFirst<ProcedureStatusDto>(
                sql: query,
                commandTimeout: TimeOutHelper.DezMinutos,
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    ExecucaoId = filter.ExecucaoId,
                    passagens = args.passagens,
                    transacoesRecusadas = args.transacoesRecusadas,
                    transacoesRecusadasParceiros = args.transacoesRecusadasParceiros,
                    veiculos = args.veiculos
                });

            Log.Debug("Executação da procedure SP_SalvarPassagensReprovadas realizada com sucesso.");

            return dto;


        }
    }
}
