using Dapper;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class SalvarPassagensReprovadasArgs
    {
        public SqlMapper.ICustomQueryParameter passagens { get; set; }
        public SqlMapper.ICustomQueryParameter transacoesRecusadas { get; set; }
        public SqlMapper.ICustomQueryParameter transacoesRecusadasParceiros { get; set; }
        public SqlMapper.ICustomQueryParameter veiculos { get; set; }
    }
}
