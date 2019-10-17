using System.Threading.Tasks;
using System.Timers;
using Common.Logging;
using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensProcessadasBatch.Management;
using LeitorPassagensProcessadasBatch.Management.Interface;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Topshelf;

namespace LeitorPassagensProcessadasBatch
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    //class Program
    //{

    //    private static ILog _log = LogManager.GetLogger(typeof(Program));

    //    private static IMonitorTransacaoQueue _monitorarTransacaoQueue;
    //    private static IMonitorTransacaoQueue MonitorarTransacaoQueue
    //    {
    //        get
    //        {

    //            if (_monitorarTransacaoQueue == null)
    //            {
    //                _log.Debug("Web Job LeitorPassagensProcessadasBatch Iniciou");
    //                _monitorarTransacaoQueue = new MonitorTransacaoQueue();
    //            }

    //            return _monitorarTransacaoQueue;
    //        }
    //    }

    //    // Please set the following connection strings in app.config for this WebJob to run:
    //    // AzureWebJobsDashboard and AzureWebJobsStorage
    //    static void Main()
    //    {
    //        var config = new JobHostConfiguration();
    //        if (config.IsDevelopment)
    //        {
    //            config.UseDevelopmentSettings();
    //        }


    //        while (true)
    //        {
    //            var task = MonitorarTransacaoQueue.Executar();
    //            while (!task.IsCompleted)
    //            {

    //            }
    //        }
    //    }
    //}

    class Program
    {

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            ConectCar.Framework.Infrastructure.Json.SerializerJson.ApplySettings(new Newtonsoft.Json.JsonSerializerSettings { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None });

            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }


            HostFactory.Run(x =>
            {
                x.Service<Executor>(s =>
                {
                    s.ConstructUsing(name => new Executor());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("[Conectcar] - LeitorPassagemProcessadasBatch - Transacoes");
                x.SetDisplayName("[Conectcar] - LeitorPassagemProcessadasBatch - Transacoes");
                x.SetServiceName("[Conectcar] - LeitorPassagemProcessadasBatch - Transacoes");
            });

        }


        public class Executor : Loggable
        {
            private readonly IMonitorTransacaoQueue _monitorarTransacaoQueue;
            private bool _processar;
            readonly Timer _timer;
            public Executor()
            {
                var count = 0;
                _monitorarTransacaoQueue = new MonitorTransacaoQueue();
                _timer = new Timer(3000) { AutoReset = true };
                _timer.Elapsed += (sender, eventArgs) =>
                {
                    if (count > 0)
                    {
                        if (!_processar)
                        {
                            _processar = true;
                            while (_processar)
                            {
                                var task = _monitorarTransacaoQueue.Executar();
                                while (!task.IsCompleted)
                                {
                                }
                            }
                        }
                    }
                    count = 1;
                };
            }
            public void Start() { _timer.Start(); }

            public void Stop()
            {
                _processar = false;
                _timer.Stop();
            }
        }



    }
}
