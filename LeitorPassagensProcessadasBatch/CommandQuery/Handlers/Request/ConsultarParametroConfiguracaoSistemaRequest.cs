
namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request
{
    public class ConsultarParametroConfiguracaoSistemaRequest
    {
        public string NomeParametro { get; }

        public ConsultarParametroConfiguracaoSistemaRequest(string nomeParametro)
        {
            NomeParametro = nomeParametro.ToString().ToUpper();
        }
    }
}
