using ConectCar.Framework.Infrastructure.Log;

namespace ProcessadorPassagensProcessadasApi.Logger
{
    public class Logger : Loggable
    {

        public void Logs(string log)
        {
            Log.Info(log);
        }
    }
}