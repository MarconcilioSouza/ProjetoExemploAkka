using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using LeitorPassagensProcessadasBatch.Processadores.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;

namespace LeitorPassagensProcessadasBatch.Processadores
{
    public sealed class ProcessadorDeMensagensArtesp :  Loggable, IProcessador
    {
        private readonly TransacaoArtespHandler _transacaoHandler;

        public ProcessadorDeMensagensArtesp() => _transacaoHandler = new TransacaoArtespHandler();

        public async Task Processar()
        {
            await ProcessarMensagensArtespAprovadasParaApi();
            await ProcessarMensagensArtespReprovadasParaApi();
            await ProcessarMensagensArtespInvalidasParaApi();
        }

        private async Task ProcessarMensagensArtespAprovadasParaApi()
        {
            await Task.Run(() =>
            {
                try
                {
                    Log.Debug(LeitorPassagensProcessadasBatchResource.InicioLeituraPassagensAprovadasArtesp);
                    var mensagensAprovadas = _transacaoHandler.Execute(new ObterMensagensAprovadasArtespRequest());
                    Log.Debug(LeitorPassagensProcessadasBatchResource.FimLeituraPassagensAprovadasArtesp);

                    if (mensagensAprovadas != null && mensagensAprovadas.Any())
                    {
                        Log.Info(String.Format(LeitorPassagensProcessadasBatchResource.QtdPassagensAprovadasArtesp, mensagensAprovadas.Count));

                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.InicioEnvioApiPassagensAprovadasArtesp, mensagensAprovadas.Count));
                        _transacaoHandler.Execute(new ProcessarAprovadasArtespRequest(mensagensAprovadas));
                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.FimEnvioApiPassagensAprovadasArtesp, mensagensAprovadas.Count));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(String.Format(LeitorPassagensProcessadasBatchResource.Error, ex), ex);
                }
                
                    
            });
        }
        
        private async Task ProcessarMensagensArtespReprovadasParaApi()
        {
            await Task.Run(() =>
            {
                try
                {
                    Log.Debug(LeitorPassagensProcessadasBatchResource.InicioLeituraPassagensReprovadasArtesp);
                    var mensagensReprovadas = _transacaoHandler.Execute(new ObterMensagensReprovadasArtespRequest());
                    Log.Debug(LeitorPassagensProcessadasBatchResource.FimLeituraPassagensAprovadasArtesp);

                    if (mensagensReprovadas.Any())
                    {
                        this.RastreamentoLogsReprovadas(mensagensReprovadas);

                        Log.Info(String.Format(LeitorPassagensProcessadasBatchResource.QtdPassagensReprovadasArtesp, mensagensReprovadas.Count));

                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.InicioEnvioApiPassagensReprovadasArtesp, mensagensReprovadas.Count));
                        _transacaoHandler.Execute(new ProcessarReprovadasArtespRequest(mensagensReprovadas));
                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.FimEnvioApiPassagensReprovadasArtesp, mensagensReprovadas.Count));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(String.Format(LeitorPassagensProcessadasBatchResource.Error, ex), ex);
                }

                
            });
        }

        private async Task ProcessarMensagensArtespInvalidasParaApi()
        {
            await Task.Run(() =>
            {
                try
                {
                    Log.Debug(LeitorPassagensProcessadasBatchResource.InicioLeituraPassagensInvalidasArtesp);
                    var mensagensInvalidas = _transacaoHandler.Execute(new ObterMensagensInvalidasArtespRequest());
                    Log.Debug(LeitorPassagensProcessadasBatchResource.FimLeituraPassagensInvalidasArtesp);

                    if (mensagensInvalidas.Any())
                    {
                        Log.Info(String.Format(LeitorPassagensProcessadasBatchResource.QtdPassagensInvalidasArtesp, mensagensInvalidas.Count));

                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.InicioEnvioApiPassagensInvalidasArtesp, mensagensInvalidas.Count));
                        _transacaoHandler.Execute(new ProcessarInvalidasArtespRequest(mensagensInvalidas));
                        Log.Debug(String.Format(LeitorPassagensProcessadasBatchResource.FimEnvioApiPassagensInvalidasArtesp, mensagensInvalidas.Count));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(String.Format(LeitorPassagensProcessadasBatchResource.Error, ex), ex);
                }


                

            });
        }


        private void RastreamentoLogsReprovadas(IList<PassagemReprovadaArtespMessage> passagemReprovadaArtespMessage)
        {
            try
            {
                //Rastreamento dos itens na tabela de logs...
                string itensGerados = passagemReprovadaArtespMessage.Select(x => x.Passagem.MensagemItemId.ToString())
                                                        .Aggregate((a, b) => String.Format("{0} - {1}", a, b));

                Log.Info($"Itens processados: {itensGerados}");

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
