using Dapper;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class SalvarPassagensAprovadasArgs
    {
        public SqlMapper.ICustomQueryParameter passagens { get; set; }
        public SqlMapper.ICustomQueryParameter transacaoPassagem { get; set; }
        public SqlMapper.ICustomQueryParameter extratos { get; set; }
        public SqlMapper.ICustomQueryParameter estonosPassagens { get; set; }
        public SqlMapper.ICustomQueryParameter extratosEstornos { get; set; }
        public SqlMapper.ICustomQueryParameter veiculos { get; set; }
        public SqlMapper.ICustomQueryParameter eventos { get; set; }
        public SqlMapper.ICustomQueryParameter viagensValePedagio { get; set; }
        public SqlMapper.ICustomQueryParameter solicitacoesImagens { get; set; }
        public SqlMapper.ICustomQueryParameter aceiteManualReenvioPassagem { get; set; }
        public SqlMapper.ICustomQueryParameter configuracaoAdesao { get; set; }
        public SqlMapper.ICustomQueryParameter divergenciasCategoriasConfirmadas { get; set; }
    }
}
