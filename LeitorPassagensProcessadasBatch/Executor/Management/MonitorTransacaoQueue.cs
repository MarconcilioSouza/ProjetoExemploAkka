using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensProcessadasBatch.CommandQuery.Mapper;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using LeitorPassagensProcessadasBatch.Management.Interface;
using LeitorPassagensProcessadasBatch.Processadores;
using LeitorPassagensProcessadasBatch.Processadores.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorPassagensProcessadasBatch.Management
{
    public class MonitorTransacaoQueue : Loggable, IMonitorTransacaoQueue
    {
        private readonly IEnumerable<IProcessador> _processadores;

        public MonitorTransacaoQueue()
        {
            MapperConfig.RegisterMappings();

            _processadores = new List<IProcessador>
            {
                new ProcessadorDeMensagensArtesp(),
                new ProcessadorDeMensagensEdi(),
                new ProcessadorDeMensagensPark(),
            };
        }

        public async Task Executar()
        {
            try
            {
                Log.Info(LeitorPassagensProcessadasBatchResource.InicioProcesso);

                foreach (var processador in _processadores)
                {
                    await processador.Processar();
                }

                Log.Info(LeitorPassagensProcessadasBatchResource.FinalProcesso);
            }
            catch (Exception ex)
            {
                
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error,ex),ex);
            }
        }
    }
}
