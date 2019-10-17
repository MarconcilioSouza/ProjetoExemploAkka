using ProcessadorPassagensActors.Actors.Artesp;
using ProcessadorPassagensActors.Actors.Edi;
using ProcessadorPassagensActors.Actors.Park;
using ProcessadorPassagensActors.CommandQuery.Mappers;
using System.Web.Http;
using ProcessadorPassagensActors.CommandQuery.Cache;

namespace ProcessadorPassagensActors
{
    public class WebApiApplication : System.Web.HttpApplication
    {


        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            MapperConfig.RegisterMappings();

            //Iniciando cache do sistema
            Cacheinitializer.Iniciar();

            //Iniciando o sistema de atores...
            TransacaoArtespActorSystem.Iniciar();
            TransacaoEdiActorSystem.Iniciar();
            TransacaoParkActorSystem.Iniciar();
        }
    }
}