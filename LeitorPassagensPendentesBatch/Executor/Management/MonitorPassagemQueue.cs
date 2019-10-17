using System;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using LeitorPassagensPendentesBatch.Management.Interface;
using System.Collections.Generic;
using LeitorPassagensPendentesBatch.Processadores.Interface;
using LeitorPassagensPendentesBatch.Processadores;

namespace LeitorPassagensPendentesBatch.Management
{
    public class MonitorPassagemQueue : Loggable, IMonitorPassagemQueue
    {
        private readonly IEnumerable<IProcessador> _processadores;

        public MonitorPassagemQueue()
        {
            _processadores = new List<IProcessador>
            {
                new ProcessadorDeMensagensArtesp(),
                new ProcessadorDeMensagensEdi(),
                new ProcessadorDeMensagensPark()
            };
        }

        /// <summary>
        /// Método responsável por verificar se existe pendencias nas filas das concessionarias e enviar para o akka
        /// </summary>
        public async Task Executar()
        {
            try
            {
                Log.Info(LeitorPassagensPendentesBatchResource.InicioProcesso);
                foreach (var processador in _processadores)
                {
                    await processador.Processar();
                }
                Log.Info(LeitorPassagensPendentesBatchResource.FinalProcesso);
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, e.Message), e);
            }
        }
    }
}
