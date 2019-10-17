using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using System.Collections.Generic;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterViagemAgendadaPorPlacaPracaDataPassagemQuery : IQuery<ObterViagemAgendadaPorPlacaPracaDataPassagemFilter, IEnumerable<DetalheViagem>>
    {
        public IEnumerable<DetalheViagem> Execute(ObterViagemAgendadaPorPlacaPracaDataPassagemFilter filter)
        {

            var query = $@"
                       SELECT dv.DetalheViagemId as Id , vg.CnpjEmbarcador, vg.CodigoViagemParceiro, vg.Embarcador,  dv.*  
                        FROM DetalheViagem dv (NOLOCK)
                        INNER JOIN Viagem vg (NOLOCK) on dv.ViagemId=vg.ViagemId                              
                        WHERE   
                             vg.Placa = @Placa
                             AND dv.PracaId = @PracaId
                             AND vg.StatusViagemId = 1
                             AND dv.StatusId = 1
                             AND vg.DataInicioViagem <= '{filter.DataPassagem:yyyy-MM-dd}'
                             AND vg.DataFimViagem >= '{filter.DataPassagem:yyyy-MM-dd}'
                             AND dv.TransacaoId IS NULL
                             AND dv.TransacaoProvisoriaId IS NULL
                        ORDER BY vg.DataFimViagem";


            if (filter.ViagemId.HasValue)
            {
                query += " AND dv.ViagemId = @ViagemId ";
            }

            query += " ORDER BY vg.DataFimViagem";

            
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var ret = conn.Query<DetalheViagemAgendadaDto>(sql: query,
                param: new
                {
                    Placa = filter.Placa,
                    PracaId = filter.PracaId,
                    DataPassagem = filter.DataPassagem,
                    ViagemId = filter.ViagemId
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                List<DetalheViagem> retorno = new List<DetalheViagem>();

                foreach (var item in ret)
                {
                    retorno.Add(new DetalheViagem
                    {
                        Id = item.DetalheViagemId,
                        TransacaoPassagemId = item.TransacaoPassagemId,
                        PracaId = item.PracaId,
                        CodigoPracaRoadCard = item.CodigoPracaRoadCard,
                        Sequencia = item.Sequencia,
                        ValorPassagem = item.ValorPassagem,
                        DataCancelamento = item.DataCancelamento,
                        StatusDetalheViagemId = item.StatusId,

                        Viagem = new Viagem()
                        {
                            Id = item.ViagemId,
                            CnpjEmbarcador = item.CnpjEmbarcador,
                            CodigoViagemParceiro = item.CodigoViagemParceiro,
                            Embarcador = item.Embarcador
                        }
                    });
                }

                return retorno;
            }
            
        }

    }
}
