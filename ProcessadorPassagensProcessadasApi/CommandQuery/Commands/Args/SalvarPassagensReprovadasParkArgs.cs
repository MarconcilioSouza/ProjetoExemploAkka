using Dapper;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    class SalvarPassagensReprovadasParkArgs
    {
        public SqlMapper.ICustomQueryParameter transacoesRecusadas { get; set; }
        public SqlMapper.ICustomQueryParameter DetalheTransacoes { get; set; }
    }
}
