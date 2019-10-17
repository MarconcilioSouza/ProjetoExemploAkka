using Common.Logging;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using LeitorPassagensPendentesBatch.Management;
using LeitorPassagensPendentesBatch.Management.Interface;
using Microsoft.Azure.WebJobs;

namespace LeitorPassagensPendentesBatch
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {

        private static ILog Log = LogManager.GetLogger(typeof(Program));

        private static IMonitorPassagemQueue _monitorarPassagemQueue;

        private static IMonitorPassagemQueue MonitorarPassagemQueue {
            get {

                if (_monitorarPassagemQueue == null)
                {
                    Log.Debug(LeitorPassagensPendentesBatchResource.Build);
                    _monitorarPassagemQueue = new MonitorPassagemQueue();
                }

                return _monitorarPassagemQueue;
            }
        }
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }
           
            var task = MonitorarPassagemQueue.Executar();
            while (!task.IsCompleted) { }
        }
    }
}
