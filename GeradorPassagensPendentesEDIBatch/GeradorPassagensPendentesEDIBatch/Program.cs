using System.Timers;
using ConectCar.Framework.Infrastructure.Log;
using GeradorPassagensPendentesEDIBatch.Management;
using GeradorPassagensPendentesEDIBatch.Management.Interfaces;
using Microsoft.Azure.WebJobs;
using Topshelf;

namespace GeradorPassagensPendentesEDIBatch
{
    //public class Program
    //{
    //    private static IGeradorPassagemPendenteEDI _geradorPassagemPendenteEdi;

    //    public static IGeradorPassagemPendenteEDI GeradorPassagemPendenteEdi
    //    {
    //        get
    //        {
    //            if(_geradorPassagemPendenteEdi == null)
    //                _geradorPassagemPendenteEdi = new GeradorPassagemPendenteEDI();

    //            return _geradorPassagemPendenteEdi;
    //        }
    //    }


    //    private static void Main()
    //    {
    //        var config = new JobHostConfiguration();
    //        if (config.IsDevelopment)
    //        {
    //            config.UseDevelopmentSettings();
    //        }

    //        while (true)
    //        {
    //            var task = GeradorPassagemPendenteEdi?.ExecuteAsync();
    //            if (task != null)
    //                while (!task.IsCompleted) { }
    //        }
    //    }

    //}

    public class Program
    {
        private static void Main()
        {
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

                x.SetDescription("[Conectcar] - GeradorPassagensPendentesBatch - Transacoes");
                x.SetDisplayName("[Conectcar] - GeradorPassagensPendentesBatch - Transacoes");
                x.SetServiceName("[Conectcar] - GeradorPassagensPendentesBatch - Transacoes");
            });
        }


        public class Executor : Loggable
        {
            private readonly IGeradorPassagemPendenteEDI _geradorPassagemPendenteEdi;
            private bool _processar;
            private readonly Timer _timer;
            public Executor()
            {
                _geradorPassagemPendenteEdi = new GeradorPassagemPendenteEDI();
                _timer = new Timer(1000) { AutoReset = true };
                _timer.Elapsed += (sender, eventArgs) =>
                {
                    if (!_processar)
                    {
                        _processar = true;
                        while (_processar)
                        {
                            var task = _geradorPassagemPendenteEdi.ExecuteAsync();
                            while (!task.IsCompleted)
                            {
                            }
                        }
                    }

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