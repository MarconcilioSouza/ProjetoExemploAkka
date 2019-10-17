using Common.Logging;
using GeradorPassagensPendentesBatch.CommandQuery.Resources;
using GeradorPassagensPendentesBatch.Management;
using GeradorPassagensPendentesBatch.Management.Interfaces;
using Microsoft.Azure.WebJobs;

namespace GeradorPassagensPendentesBatch
{
    class Program
    {
        private static ILog Log = LogManager.GetLogger(typeof(Program));


        private static IGeradorPassagensPendentes _geradorPassagensPendentes;
        private static IGeradorPassagensPendentes GeradorPassagensPendentes {
            get
            {
                if (_geradorPassagensPendentes == null)
                {
                    Log.Debug(GeradorPassagemPendenteResource.Build);
                    _geradorPassagensPendentes = new GeradorPassagensPendentes();
                }
                    

                return _geradorPassagensPendentes;
            }
        }

        static void Main()
        {
            
            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var task = GeradorPassagensPendentes.ExecuteAsync();
            while (!task.IsCompleted) { }
        }
    }
}
