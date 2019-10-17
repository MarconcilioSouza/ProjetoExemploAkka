using System.Data;
using System.Linq;
using AutoMapper;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Filter;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;
using System;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class CriarPassagemCompletaEdiQuery : DbConnectionQueryBase<PassagemPendenteEDI, PassagemPendenteEDI>
    {
        private readonly DbConnectionDataSource _dataSource;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        public CriarPassagemCompletaEdiQuery(DbConnectionDataSource dataSource, DbConnectionDataSource dataSourceFallBack) : base(dataSource)
        {
            _dataSource = dataSource;
            _dataSourceFallBack = dataSourceFallBack;
        }

        public override PassagemPendenteEDI Execute(PassagemPendenteEDI passagemPendenteEdi)
        {
            
            var tagAdesaoDto = DataSource.Connection.Query<TagAdesaoDto>(
                "[dbo].[spObterTagAdesaoEDI]",
                new
                {
                    passagemPendenteEdi.Tag.OBUId,
                    DataReferenciaCancelamento = passagemPendenteEdi.DataPassagem
                },
                commandType: CommandType.StoredProcedure
                ,commandTimeout: TimeHelper.CommandTimeOut
                ).FirstOrDefault();

            if (tagAdesaoDto != null)
                Mapper.Map(tagAdesaoDto, passagemPendenteEdi);

            passagemPendenteEdi.Tag = passagemPendenteEdi.Adesao.Tag;
            
            var obterPistaPracaCache = new ObterPistaPracaConveniadoEDIQuery(true, _dataSource, _dataSourceFallBack);

            var pistaPraca = obterPistaPracaCache.Execute(new ConveniadoEDIFilter
            {
                CodigoProtocolo = passagemPendenteEdi.Conveniado.CodigoProtocolo,
                CodigoPraca = passagemPendenteEdi.Praca.CodigoPraca,
                CodigoPista = passagemPendenteEdi.Pista.CodigoPista
            }).FirstOrDefault();

            if (pistaPraca != null)
                Mapper.Map(pistaPraca, passagemPendenteEdi);

            var carregarCategoria = new ObterEcarregarCategoriaVeiculoPorCodigoEdiQuery();
            carregarCategoria.Execute(passagemPendenteEdi, _dataSource);

            return passagemPendenteEdi;
        }
    }
}
