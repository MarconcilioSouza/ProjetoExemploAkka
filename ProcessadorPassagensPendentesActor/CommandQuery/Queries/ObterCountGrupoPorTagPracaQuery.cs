using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountGrupoPorTagPracaQuery : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountGrupoPorTagPracaQuery()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            var query = @"Select count(*) from TagGrupoPraca tgp (nolock)
                            where tgp.TagId = @TagId and PracaId = @PracaId";


            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>(
                                    sql: query,
                                    param: new
                                    {
                                        TagId = filter.Tag.Id,
                                        PracaId = filter.Praca.Id
                                    },
                                    commandTimeout: TimeHelper.CommandTimeOut);
                return result > 0;
            }
                
        }
    }
}
