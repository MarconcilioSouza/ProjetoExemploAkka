using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using LeitorPassagensProcessadasBatch.Processadores.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeitorPassagensProcessadasBatch.Processadores
{
    public sealed class ProcessadorDeMensagensEdi : Loggable, IProcessador
    {
        private readonly TransacaoEdiHandler _transacaoHandler;

        public ProcessadorDeMensagensEdi() => _transacaoHandler = new TransacaoEdiHandler();

        public async Task Processar()
        {
            await ProcessarMensagensAprovadasEdiParaApi();
            await ProcessarMensagensReprovadasEdiParaApi();
        }

        private async Task ProcessarMensagensAprovadasEdiParaApi()
        {
            await Task.Run(() =>
            {
                //  Carregamos todas as mensagens da fila aprovada e enviamos para a API.
                Log.Debug(LeitorPassagensProcessadasBatchResource.InicioLeituraPassagensAprovadasEDI);
                var mensagensAprovadas = _transacaoHandler.Execute(new ObterMensagensAprovadasEdiRequest());
                Log.Debug(LeitorPassagensProcessadasBatchResource.FimLeituraPassagensAprovadasEDI);

                if (mensagensAprovadas != null && mensagensAprovadas.Any())
                {
                    Log.Info(string.Format(LeitorPassagensProcessadasBatchResource.QtdPassagensAprovadasEDI, mensagensAprovadas.Count));
                    Log.Debug(string.Format(LeitorPassagensProcessadasBatchResource.InicioEnvioApiPassagensAprovadasEDI, mensagensAprovadas.Count));
                    _transacaoHandler.Execute(new ProcessarAprovadasEdiRequest(mensagensAprovadas));
                    Log.Debug(string.Format(LeitorPassagensProcessadasBatchResource.FimEnvioApiPassagensAprovadasEDI, mensagensAprovadas.Count));
                }
            });
        }

        private async Task ProcessarMensagensReprovadasEdiParaApi()
        {
            await Task.Run(() =>
            {
                //  Carregamos todas as mensagens da fila reprovada e enviamos para a API.
                Log.Debug(LeitorPassagensProcessadasBatchResource.InicioLeituraPassagensReprovadasEDI);
                var mensagensReprovadas = _transacaoHandler.Execute(new ObterMensagensReprovadasEdiRequest());
                Log.Debug(LeitorPassagensProcessadasBatchResource.FimLeituraPassagensReprovadasEDI);

                if (mensagensReprovadas != null && mensagensReprovadas.Any())
                {
                    Log.Info(string.Format(LeitorPassagensProcessadasBatchResource.QtdPassagensReprovadasEDI, mensagensReprovadas.Count));
                    Log.Debug(string.Format(LeitorPassagensProcessadasBatchResource.InicioEnvioApiPassagensReprovadasEDI, mensagensReprovadas.Count));
                    _transacaoHandler.Execute(new ProcessarReprovadasEdiRequest(mensagensReprovadas));
                    Log.Debug(string.Format(LeitorPassagensProcessadasBatchResource.FimEnvioApiPassagensReprovadasEDI, mensagensReprovadas.Count));
                }
            });
        }
    }
}
