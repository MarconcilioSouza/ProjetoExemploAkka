using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using System.Data;
using System.Linq;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class CriarPassagemCompletaParkQuery : DbConnectionQueryBase<PassagemPendenteEstacionamento, PassagemPendenteEstacionamento>
    {
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;

        public CriarPassagemCompletaParkQuery(DbConnectionDataSource dataSourceConectSysReadOnly, DbConnectionDataSource dataSourceFallBack)
            : base(dataSourceConectSysReadOnly)
        {
            _dataSourceConectSysReadOnly = dataSourceConectSysReadOnly;
            _dataSourceFallBack = dataSourceFallBack;
        }

        public override PassagemPendenteEstacionamento Execute(PassagemPendenteEstacionamento ppEstacionamento)
        {
            var tagAdesaoDto = DataSource.Connection.Query<TagAdesaoDto>(
                "[dbo].[spObterTagAdesaoEDI]",
                new
                {
                    ppEstacionamento.Tag.OBUId,
                    DataReferenciaCancelamento = ppEstacionamento.DataPassagem
                },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (tagAdesaoDto != null)
                Mapper.Map(tagAdesaoDto, ppEstacionamento);

            ppEstacionamento.Tag = ppEstacionamento.Adesao.Tag;

            
            var conveniado = DataSource.Connection.Query<Conveniado>(
                @"SELECT 	c.ConveniadoId as Id,c.CodigoProtocolo,c.CodigoProtocoloArtesp,pn.NomeFantasia,c.NomeFatura,pn.RazaoSocial
		                    ,c.HabilitarValidacaoTarifa,c.UtilizaRps,c.ListaDeParaCategoriaVeiculoId,c.HabilitarConfirmacaoCategoria
		                    ,c.AtivoProtocoloArtesp,c.TempoDeCorrecaoDasTransacoesProvisorias
	            FROM Conveniado c (nolock)
	            INNER JOIN ParceiroNegocio pn (nolock) ON pn.ParceiroNegocioId = c.ConveniadoId
	            WHERE c.StatusID = 1 -- Ativo
	            AND TipoParceiroNegocioId IN (2,3) -- 2 - Estacionamento / 3 - Shopping
	            AND CodigoProtocolo = @CodigoProtocolo",
                new
                {
                    CodigoProtocolo = ppEstacionamento.Conveniado.CodigoProtocolo,
                },
               commandType: CommandType.Text)
               .FirstOrDefault();

            Praca praca = null;
            Pista pista = null;

            if (conveniado != null)
            {
                Mapper.Map(conveniado, ppEstacionamento);

                if (ppEstacionamento.Praca.CodigoPraca.HasValue)
                {
                    praca = DataSource.Connection.Query<Praca>(
                          @"SELECT PracaId as Id, CodigoPraca,IdentificacaoPraca, TempoAtualizacaoPista, TempoRetornoPraca
                                TempoAtualizacaoTransacao
                            FROM Praca (nolock)
	                        WHERE ConveniadoId = @ConveniadoId
                            AND CodigoPraca = @CodigoPraca",
                       new
                       {
                           ConveniadoId = ppEstacionamento.Conveniado.Id,
                           CodigoPraca = ppEstacionamento.Praca.CodigoPraca.Value,
                       },
                      commandType: CommandType.Text)
                      .FirstOrDefault();

                    if (praca != null)
                    {
                        Mapper.Map(praca, ppEstacionamento);

                        if (ppEstacionamento.Pista.CodigoPista > 0)
                        {
                            pista = DataSource.Connection.Query<Pista>(
                                  @"SELECT PistaId as Id, CodigoPista
                                    FROM Pista (nolock)
	                                WHERE Pracaid = @Pracaid
                                    AND CodigoPista = @CodigoPista",
                                  new
                                  {
                                      Pracaid = ppEstacionamento.Praca.Id,
                                      CodigoPista = ppEstacionamento.Pista.CodigoPista,
                                  },
                                  commandType: CommandType.Text)
                                  .FirstOrDefault();

                            if (pista != null)
                            {
                                Mapper.Map(pista, ppEstacionamento);
                            }
                        }
                    }
                }
                
            }
            return ppEstacionamento;
        }
    }
}
