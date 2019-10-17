using System;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ListarViagensASeremCanceladasQuery : IQuery<DetalheViagem, List<DetalheViagem>>
    {
        public ListarViagensASeremCanceladasQuery()
        {
        }

        public List<DetalheViagem> Execute(DetalheViagem filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<DetalheViagemDto>(
                    @"SELECT dv.DetalheViagemId as Id, * FROM DetalheViagem dv (NOLOCK)
		                WHERE dv.ViagemId = @viagemId
		                AND dv.StatusId not in (3,5,2)
		                AND dv.Sequencia = @sequencia",
                    new
                    {
                        viagemId = filter.Viagem.Id,
                        sequencia = filter.Sequencia

                    },
                    commandTimeout: TimeHelper.CommandTimeOut).ToList();

                if (result.Any())
                {
                    return (from viagem in result
                            select new DetalheViagem
                            {
                                PracaId = viagem.PracaId,
                                Id = viagem.Id,
                                CodigoPracaRoadCard = viagem.CodigoPracaRoadCard,
                                DataCancelamento = DateTime.Now,
                                Sequencia = viagem.Sequencia,
                                StatusDetalheViagemId = viagem.StatusId,
                                ValorPassagem = viagem.ValorPassagem,
                                Viagem = new Viagem { Id = viagem.ViagemId }
                            }).ToList();

                }

                return new List<DetalheViagem>();
            }

        }


    }
}
