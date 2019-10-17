using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterNumeroVezesRecusadoParamValePedagioFinanceiroQuery
    {

        private ObterParametrosValePedagioFinanceiroQuery _parametrosValePedagioFinanceiroQuery;

        public ObterNumeroVezesRecusadoParamValePedagioFinanceiroQuery()
        {
            _parametrosValePedagioFinanceiroQuery = new ObterParametrosValePedagioFinanceiroQuery();
        }

        public int Execute()
        {
            var parametro = DataBaseConnection.HandleExecution(_parametrosValePedagioFinanceiroQuery.Execute);
            return parametro;
        }
    }
}
