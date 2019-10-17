using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTarifaPorPracaECategoria : IQuery<PassagemPendenteArtesp, decimal>
    {
        public ObterTarifaPorPracaECategoria()
        {
        }

        public decimal Execute(PassagemPendenteArtesp filter)
        {
            var query = @"
                        SELECT TOP 1 Valor FROM Tarifa t (NOLOCK)
                            WHERE t.PracaId = @PracaId
                            AND t.CategoriaVeiculoId = @CategoriaVeiculoId
                            AND t.VigenciaInicio <= @DataPassagem
                            AND t.GrupoId = @GrupoId
                            ORDER BY t.VigenciaInicio DESC
                        ";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var ret = conn.Query<decimal>(sql: query,
               param: new
               {
                   PracaId = filter.Praca.Id,
                   CategoriaVeiculoId = filter.CategoriaCobrada.Id,
                   DataPassagem = filter.DataPassagem,
                   GrupoId = filter.Tag.GrupoPadraoId
               },
               commandTimeout: TimeHelper.CommandTimeOut);

                return ret.FirstOrDefault();
            }


        }


    }
}
