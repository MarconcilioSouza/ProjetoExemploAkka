using ProcessadorPassagensProcessadasApi.CommandQuery.Mappers;
using System.Web.Http;


namespace ProcessadorPassagensProcessadasApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        Logger.Logger log = new Logger.Logger();

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            MapperConfig.RegisterMappings();

            log.Logs("Iniciando sistema de passagens processadas...");
        }
    }
}
