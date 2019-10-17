using LeitorPassagensProcessadasBatch.CommandQuery.Handlers;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park;
using LeitorPassagensProcessadasBatch.Processadores.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace LeitorPassagensProcessadasBatch.Processadores
{
    public sealed class ProcessadorDeMensagensPark : IProcessador
    {
        private readonly TransacaoParkHandler _transacaoHandler;
        public ProcessadorDeMensagensPark() => _transacaoHandler = new TransacaoParkHandler();

        public async Task Processar()
        {
            await ProcessarMensagensParkAprovadasParaApi();
            await ProcessarMensagensParkReprovadasParaApi();
        }

        private async Task ProcessarMensagensParkAprovadasParaApi()
        {
            await Task.Run(() =>
            {
                //  Carregamos todas as mensagens da fila aprovada e enviamos para a API.
                var mensagensAprovadas = _transacaoHandler.Execute(new ObterMensagensAprovadasParkRequest());

                if (mensagensAprovadas.Any())
                {
                    _transacaoHandler.Execute(new ProcessarAprovadasParkRequest(mensagensAprovadas));
                }
            });
        }

        private async Task ProcessarMensagensParkReprovadasParaApi()
        {
            await Task.Run(() =>
            {
                //  Carregamos todas as mensagens da fila reprovada e enviamos para a API.
                var mensagensReprovadas = _transacaoHandler.Execute(new ObterMensagensReprovadasParkRequest());

                if (mensagensReprovadas.Any())
                    _transacaoHandler.Execute(new ProcessarReprovadasParkRequest(mensagensReprovadas));

            });
        }
    }
}

