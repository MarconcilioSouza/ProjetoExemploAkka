using Dapper;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class SalvarPassagensAprovadasParkArgs
    {
        public SqlMapper.ICustomQueryParameter transacoes { get; set; }
        public SqlMapper.ICustomQueryParameter DetalhePassagens { get; set; }
        public SqlMapper.ICustomQueryParameter PistaInformacoes { get; set; }
        public SqlMapper.ICustomQueryParameter ConveniadoInformacoes { get; set; }
    }
}
