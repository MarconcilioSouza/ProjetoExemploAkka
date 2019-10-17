using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.Infrastructure
{
    public static class PoolRouterHelper
    {
        public static int Lower => ConfigurationManager.AppSettings["DefaultResizer_Lower"].TryToInt();
        public static int Upper => ConfigurationManager.AppSettings["DefaultResizer_Upper"].TryToInt();
        public static int MessagesPerResize => ConfigurationManager.AppSettings["DefaultResizer_MessagesPerResize"].TryToInt();
        public static int NrOfInstances => ConfigurationManager.AppSettings["Pool_NrOfInstances"].TryToInt();


    }
}
