using System.Configuration;

namespace ProcessadorPassagensActors.Infrastructure
{
    public static class TimeHelper
    {
        public static int CommandTimeOut => ConfigurationManager.AppSettings["CommandTimeOut"].TryToInt();  
        public static int LagSeconds => ConfigurationManager.AppSettings["LagSeconds"].TryToInt(); 
        public static int CacheExpiration => ConfigurationManager.AppSettings["CacheExpiration"].TryToInt(); //expiration

    }
}
