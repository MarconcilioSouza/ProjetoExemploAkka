using ConectCar.Framework.Infrastructure.Log;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public  class ActorLogger : Loggable
    {
        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }
    }
}