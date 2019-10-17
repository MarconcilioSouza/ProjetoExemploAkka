using GeradorPassagensPendentesParkBatch.Management;
using GeradorPassagensPendentesParkBatch.Management.Interfaces;
using Microsoft.Azure.WebJobs;

namespace GeradorPassagensPendentesParkBatch
{
    public class Program
    {
        private static IGeradorPassagemPendentePark _geradorPassagemPendentePark { get; set; }

        private static void Main()
        {
            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            _geradorPassagemPendentePark = new GeradorPassagemPendentePark();

            while (true)
            {
                var task = _geradorPassagemPendentePark.ExecuteAsync();
                while (!task.IsCompleted) { }
            }

        }
    }
}