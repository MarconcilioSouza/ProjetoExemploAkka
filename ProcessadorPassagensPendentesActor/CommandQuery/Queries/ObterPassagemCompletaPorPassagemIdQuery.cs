using System.Data;
using System.Linq;
using AutoMapper;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemCompletaPorPassagemIdQuery : IQuery<ObterPassagemCompletaFilter, PassagemPendenteArtesp>
    {
        
        public ObterPassagemCompletaPorPassagemIdQuery()
        {
            
        }

        public PassagemPendenteArtesp Execute(ObterPassagemCompletaFilter filter)
        {

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                
                var passagemDto = conn.Query<PassagemAnteriorValidaDto>(
                    "[dbo].[spObterTagAdesaoPorPassagemId]",
                    new
                    {
                        filter.PassagemId
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

               
                if (passagemDto != null)
                    Mapper.Map(passagemDto, filter.PassagemPendenteArtesp);

                var pistaPracaConveniados = PistaPracaConveniadoArtespCacheRepository.Listar();
                var pistaPraca = pistaPracaConveniados.FirstOrDefault(x=>
                    x.CodigoProtocoloArtesp == filter.PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp
                    && x.CodigoPraca == filter.PassagemPendenteArtesp.Praca.CodigoPraca
                    && x.CodigoPista == filter.PassagemPendenteArtesp.Pista.CodigoPista
                );                

                if (pistaPraca != null)
                    Mapper.Map(pistaPraca, filter.PassagemPendenteArtesp);

                return filter.PassagemPendenteArtesp;
            }                
        }
    }
}
